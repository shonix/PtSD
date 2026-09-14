(* File Arm64/Arm64Comp.sml

   Micro-C compiler that generates an Arm64 assembler-oriented bytecode.

   sestoft@itu.dk * 2026-01-30 based on Kokholm & Sestoft 2002-2017

   Differences from MicroC/Comp.fs:

    * Uses Arm64.fs for code emission instead of Machine.fs.

    * The label of a function entry point is of type flabel instead of
      label. This changes type funEnv. The label for function "fname"
      is "fname" (or "_fname" on MacOS) instead of a label created by
      newLabel(). Also a change in function cProgram.

    * The argc is added to the return type of cProgram so that
      compileToFile can insert code to check the number of
      arguments. This eliminates a source of mysterious crashes.

    The created assembly file can be translated (by clang) to binary
    assembly code, defining the entry point asm_main which will
    convert command line arguments and call the compiled main
    function. This file can be linked (by clang) together with the
    compiled driver.o, written in C, to a complete executable file.

    This has been tested on a Macbook MacOS 15.7.3 with clang 17.0.0,
    and on Raspberry Pi 5 Linux 6.12 and clang 19.1.7.

    For a general description of the compiler, see chapter [TODO] of
    Programming Language Concepts, third edition, 2026.
*)

module Arm64Comp

open System.IO
open Absyn
open Arm64

(* ------------------------------------------------------------------- *)

(* Simple environment operations *)

type 'data env = (string * 'data) list

let rec lookup env x = 
    match env with 
    | []         -> failwith (x + " not found")
    | (y, v)::yr -> if x=y then v else lookup yr x

(* A global variable has a fixed address, a local one has an offset: *)

type var = 
    Glovar of int                   (* address relative to bottom of stack *)
  | Locvar of int                   (* address relative to bottom of frame *)

(* The variable environment keeps track of global and local variables,
   and keeps track of next available variable index, which equals the
   number of variables and array places allocated so far *)

type varEnv = (var * typ) env * int

(* The function environment maps function name to label and parameter decs *)

type paramdecs = (typ * string) list
type funEnv = (flabel * typ option * paramdecs) env

(* Bind declared variable in env and generate code to allocate it *)

let allocate (kind : int -> var) (typ, x) (varEnv : varEnv) : varEnv * arm64 list =
    let (env, fdepth) = varEnv 
    match typ with
    | TypA (TypA _, _) -> 
      failwith "allocate: array of arrays not permitted here"
    | TypA (t, Some i) ->
      (* Allocate 8 bytes per array element, padding for 16-byte alignment *)
      let ialign = if i % 2 = 1 then i+1 else i
      let newEnv = ((x, (kind (fdepth+ialign/2), typ)) :: env, fdepth+ialign/2+1)
      let code = [Mov(X8, Reg Sp); 
                  Arith("sub", X8, X8, Cst 16); 
                  Arith("sub", Sp, Sp, Cst (8L * (int64)ialign));
                  Push X8]
      (newEnv, code) 
    | _ -> 
      let newEnv = ((x, (kind fdepth, typ)) :: env, fdepth+1)
      let code = [Arith("sub", Sp, Sp, Cst 16)]
      (newEnv, code)

(* Bind declared parameters in env: *)

let bindParam (env, fdepth) (typ, x)  : varEnv = 
    ((x, (Locvar fdepth, typ)) :: env , fdepth+1)

let bindParams paras ((env, fdepth) : varEnv) : varEnv = 
    List.fold bindParam (env, fdepth) paras;

(* ------------------------------------------------------------------- *)

(* Global environments for variables and functions *)

let makeGlobalEnvs (topdecs : topdec list) : varEnv * funEnv * arm64 list = 
    let rec addv decs varEnv funEnv = 
        match decs with 
        | []         -> (varEnv, funEnv, [])
        | dec::decr  -> 
          match dec with
          | Vardec (typ, var) ->
            let (varEnv1, code1)          = allocate Glovar (typ, var) varEnv
            let (varEnvr, funEnvr, coder) = addv decr varEnv1 funEnv
            (varEnvr, funEnvr, code1 @ coder)
          | Fundec (tyOpt, f, xs, body) ->
            addv decr varEnv ((f, ("_" + f, tyOpt, xs)) :: funEnv)
    addv topdecs ([], 0) []

(* ------------------------------------------------------------------- *)

(* Compiling micro-C statements *)

let rec cStmt stmt (varEnv : varEnv) (funEnv : funEnv) : arm64 list = 
    match stmt with
    | If(e, stmt1, stmt2) -> 
      let labelse = newLabel()
      let labend  = newLabel()
      cExpr e varEnv funEnv X8 []
      @ [Cbz(X8, labelse)] 
      @ cStmt stmt1 varEnv funEnv
      @ [B labend]
      @ [Label labelse] @ cStmt stmt2 varEnv funEnv
      @ [Label labend]           
    | While(e, body) ->
      let labbegin = newLabel()
      let labtest  = newLabel()
      [B labtest; Label labbegin]
      @ cStmt body varEnv funEnv
      @ [Label labtest] @ cExpr e varEnv funEnv X8 []
      @ [Cbnz(X8, labbegin)]
    | Expr e -> 
      cExpr e varEnv funEnv X8 []
    | Block stmts -> 
      let rec loop stmts varEnv =
          match stmts with 
          | []     -> (snd varEnv, [])
          | s1::sr -> 
            let (varEnv1, code1) = cStmtOrDec s1 varEnv funEnv
            let (fdepthr, coder) = loop sr varEnv1 
            (fdepthr, code1 @ coder)
      let (fdepthend, code) = loop stmts varEnv
      code @ [Arith("add", Sp, Sp, Cst (16L * (int64)(fdepthend - snd varEnv)))]
    | Return None ->
      Arith("add", Sp, Sp, Cst (16L * (int64)(snd varEnv))) :: popreturn
    | Return (Some e) -> 
      cExpr e varEnv funEnv X0 [] 
      @ Arith("add", Sp, Sp, Cst (16L * (int64)(snd varEnv))) :: popreturn

and cStmtOrDec stmtOrDec (varEnv : varEnv) (funEnv : funEnv) : varEnv * arm64 list = 
    match stmtOrDec with 
    | Stmt stmt    -> (varEnv, cStmt stmt varEnv funEnv) 
    | Dec (typ, x) -> allocate Locvar (typ, x) varEnv

(* Compiling micro-C expressions: 

   * e       is the expression to compile
   * varEnv  is the local and gloval variable environment 
   * funEnv  is the global function environment
   * tr      is the Arm64 register in which the result should be computed
   * pres    is a list of registers that must be preserved during the computation
   
   Net effect principle: if the compilation (cExpr e varEnv funEnv tr pres) of
   expression e returns the instruction sequence instrs, then the
   execution of instrs will leave the rvalue of expression e in register tr,
   leave the registers in pres unchanged, and leave the net stack depth unchanged.
*)

and cExpr (e : expr) (varEnv : varEnv) (funEnv : funEnv) (tr : reg64) (pres : reg64 list) : arm64 list = 
    let stackDepth = snd varEnv + List.length pres
    match e with
    | Access acc     ->
      cAccess acc varEnv funEnv tr pres @ [Ldr(tr, tr)] 
    | Assign(acc, e) ->
      let tr' = getTempFor (tr :: pres) 
      cAccess acc varEnv funEnv tr' (tr :: pres)
      @ cExpr e varEnv funEnv tr (tr' :: pres)
      @ [Str(tr, tr')]
    | CstI i         -> [Mov(tr, Cst i)]
    | Addr acc       -> cAccess acc varEnv funEnv tr pres
    | Prim1(ope, e1) ->
      match ope with
      | "!"      -> cExpr e1 varEnv funEnv tr pres @ [Cmp(tr, Xzr); Cset(tr, "ne")]
      | "printi" -> preserveAll pres (cExpr e1 varEnv funEnv tr pres @ [Mov(X0, Reg tr); Printi])
      | "println" -> preserveAll pres (cExpr e1 varEnv funEnv tr pres @ [Mov(X0, Reg tr); Println])
      | _        -> failwith "unknown primitive 1"
    | Prim2(ope, e1, e2) ->
      cExpr e1 varEnv funEnv tr pres
      @ let tr' = getTempFor (tr :: pres)
        in cExpr e2 varEnv funEnv tr' (tr :: pres) 
           @ match ope with
             | "+"   -> [Arith("add",  tr, tr, Reg tr')]
             | "-"   -> [Arith("sub",  tr, tr, Reg tr')]
             | "*"   -> [Arith("mul",  tr, tr, Reg tr')]
             | "/"   -> [Arith("sdiv", tr, tr, Reg tr')]
             | "%"   -> let tr'' = getTempFor (tr' :: tr :: pres)
                        [Arith("udiv", tr'', tr, Reg tr'); Msub(tr, tr'', tr', tr)] 
             | "==" | "!=" | "<" | ">=" | ">" | "<="
                -> let setcompbit = (match ope with
                                     | "==" -> "eq"
                                     | "!=" -> "ne"
                                     | "<"  -> "lt"
                                     | ">=" -> "ge"
                                     | ">"  -> "gt"
                                     | "<=" -> "le"
                                     | _    -> failwith "internal error")
                   [Cmp(tr, tr'); Cset(tr, setcompbit)]
             | _     -> failwith "unknown primitive 2"
    | Andalso(e1, e2) ->
      let labend = newLabel()
      cExpr e1 varEnv funEnv tr pres
      @ [Cbz(tr, labend)]
      @ cExpr e2 varEnv funEnv tr pres
      @ [Label labend] 
    | Orelse(e1, e2) -> 
      let labend = newLabel()
      cExpr e1 varEnv funEnv tr pres
      @ [Cbnz(tr, labend)]
      @ cExpr e2 varEnv funEnv tr pres
      @ [Label labend] 
    | Call(f, es) -> 
      let (labf, _, paramdecs) = lookup funEnv f
      if List.length es = List.length paramdecs then 
          preserveAll pres
             (cExprs es argumentRegisters varEnv funEnv []
              @ [Bl labf; Mov(tr, Reg X0)])
      else
          failwith (f + ": parameter/argument mismatch")

(* Generate code to access a variable, dereference a pointer or index an array: *)

and cAccess access varEnv funEnv (tr : reg64) (pres : reg64 list) : arm64 list =
    match access with 
    | AccVar x ->
      match lookup (fst varEnv) x with
      | Glovar addr, _ -> [Mov(tr, Cst ((int64)addr+1L)); Arith("sub", tr, X28, Off16 tr)] 
      | Locvar addr, _ -> [Mov(tr, Cst ((int64)addr+1L)); Arith("sub", tr, X29, Off16 tr)]       
    | AccDeref e -> cExpr e varEnv funEnv tr pres
    | AccIndex(acc, idx) ->
      cAccess acc varEnv funEnv tr pres
      @ [Ldr(tr, tr)] 
      @ let tr' = getTempFor (tr :: pres) 
        in cExpr idx varEnv funEnv tr' (tr :: pres) @ [Arith("sub", tr, tr, Off8 tr')]

(* Generate code to evaluate expressions es, putting their values in registers rs: *)

and cExprs (es : expr list) (rs : reg64 list) varEnv funEnv (pres : reg64 list) : arm64 list =
    match (es, rs) with
    | ([], _)            -> []
    | (e :: er, r :: rr) -> 
      cExpr e varEnv funEnv r pres @ cExprs er rr varEnv funEnv (r :: pres)
    | (e :: er, [])      -> failwith "too many function arguments"

(* Compile a complete micro-C program: globals, call to main, functions *)

let cProgram (Prog topdecs) : arm64 list * int * arm64 list * arm64 list = 
    let _ = resetLabels ()
    let ((globalVarEnv, globalCount), funEnv, globalInit) = makeGlobalEnvs topdecs 
    let compilefun (tyOpt, f, xs, body) =
        let (labf, _, paras) = lookup funEnv f
        let (envf, fdepthf) = bindParams paras (globalVarEnv, 0)
        let code = cStmt body (envf, fdepthf) funEnv
        let arity = List.length paras
        [FLabel (labf, arity)] @ code @ Arith("add", Sp, Sp, Cst(16L * (int64)arity)) :: popreturn
    let functions = 
        List.choose (function 
                         | Fundec (rTy, name, argTy, body) 
                                    -> Some (compilefun (rTy, name, argTy, body))
                         | Vardec _ -> None)
                    topdecs 
    let (mainlab, _, mainparams) = lookup funEnv "main"
    let argc = List.length mainparams
    (globalInit,
     argc, 
     [Bl mainlab],
     List.concat functions)

(* Compile a complete micro-C program and write the resulting assembly code
   file fname; also, return the program as a list of instructions.  *)

let asmToFile (inss : string list) (fname : string) : unit = 
    File.WriteAllText(fname, String.concat "" (List.map string inss))

let compileToFile program source fname = 
    let (globalinit, argc, maincall, functions) = cProgram program 
    let code = ["// Generated by the MicroC Arm64 compiler from file " + source + "\n"]
               @ [Arm64.stdheader; Arm64.beforeinit argc]
               @ Arm64.code2arm64asm globalinit
               @ [Arm64.loadmainargs argc]
               @ Arm64.code2arm64asm maincall
               @ [Arm64.popglobals]
               @ Arm64.code2arm64asm functions
    asmToFile code fname;
    functions 

(* Example MicroC programs are found in the files ex0.c, ex1.c, ex2.c, etc *)
