(* File Arm64/Arm64.fs

   Instructions and assembly code for Arm64 on Linux and MacOS.
   sestoft@itu.dk * 2026-07-27, based on X86.fs (Kokholm, Sestoft)

   Overall design:

   * We use only 64-bit integer registers x0-x31, 64-bit stack
     positions, 64-bit pointers, and arrays of 64-bit values.

   * Expressions are compiled to register-based code without use of
     the stack, using registers x0-x7 for passing function arguments
     (to C and MicroC functions), x8 for temporary data, x9-x17 for
     subexpression values, x28 as the MicroC global base pointer, and
     finally, as is the Arm64 convention, x29 for frame base pointer
     and x30 for storing the return address.  Registers x18-x27 are
     not used by the generated code.

   * The function arguments passed in registers x0-x7 are copied to
     the stack by the called function.  A function's result is passed
     back to the caller in register x0.

   * Both function arguments and local variables are put the stack.
     This permits computing the address &x also of an parameter x.
     All are 16-byte aligned despite using only 8 bytes each, and
     indexed off the frame base pointer in register x29.  Arrays are
     stored in the stack, each array element is 8 bytes, and an array
     with an odd number of elements is padded with an extra one.

   * Thus the stack pointer sp is 16-byte aligned at every stack
     access, also function calls and function returns.

   * There is no optimized register allocation across expressions and
     statements.

   * We use the native Arm64 call (bl) and ret instructions and its
     conventions for saving old base pointer from x29 and return
     address from x30 to the stack, where the base pointer in x29
     points at the previous (enclosing function call's) base pointer.
*)

module Arm64

(* The linker on MacOS, but not on Linux and Windows, expects an
   underscore (_) before external and global names.  So on MacOS, what
   is called foo in C must be called _foo in Arm64 assembly code.
*)

let isMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX)

let prefix = if isMacOS then "_" else ""

let printi    = prefix + "printi"
let println   = prefix + "println"
let checkargc = prefix + "checkargc"
let asm_main  = prefix + "asm_main"

type label = string

type flabel = string

type reg64 =
    X0 | X1 | X2 | X3 | X4 | X5 | X6 | X7 | X8 | X9 | X10 | X11 | X12 | X13 | X14 | X15 | X16 | X17 | X18 | X19 | X20 | X21 | X22 | X23 | X24 | X25 | X26 | X27 | X28 | X29 | X30 | Sp | Xzr

type cc =
    Eq | Ge | Gt | Le | Lt | Ne

type rand =
    | Cst of int64                      (* integer constant           *)
    | Off8 of reg64                     (* register offset x9, lsl 3  *)
    | Off16 of reg64                    (* register offset x9, lsl 4  *)
    | Reg of reg64                      (* register x9                *)
    
type arm64 =
    | Arith of string * reg64 * reg64 * rand (* add x9, x9, x20       *)
    | B of label                        (* b lab                      *)
    | Bl of label                       (* bl lab                     *)
    | Cbnz of reg64 * label             (* cbnz rn, lab               *)
    | Cbz of reg64 * label              (* cbz rn, lab                *)    
    | Cmp of reg64 * reg64              (* cmp x9, xzr                *)
    | Cset of reg64 * string            (* cset x9, lt                *)
    | FLabel of flabel * int            (* function label, arity      *)
    | Ins of string                     (* ret                        *)
    | Label of label                    (* symbolic label             *)
    | Ldr of reg64 * reg64              (* ldr x9, [x10]              *)
    | Mov of reg64 * rand               (* mov x9, x10; mov x9, 240   *)
    | Msub of reg64 * reg64 * reg64 * reg64  (* msub x9, x9, x10, x11 *)
    | Pop of reg64                      (* ldr x9, [sp], 16           *)
    | Printi                            (* print [sp] as long         *)
    | Println                           (* print newline              *)
    | Push of reg64                     (* str x9, [sp, -16]!         *)
    | Str of reg64 * reg64              (* str x9, [x10]              *)

let fromReg reg =
    match reg with
        | X0  -> "x0"
        | X1  -> "x1"
        | X2  -> "x2"
        | X3  -> "x3"
        | X4  -> "x4"
        | X5  -> "x5"
        | X6  -> "x6"
        | X7  -> "x7"
        | X8  -> "x8"
        | X9  -> "x9"
        | X10 -> "x10"
        | X11 -> "x11"
        | X12 -> "x12"
        | X13 -> "x13"
        | X14 -> "x14"
        | X15 -> "x15"
        | X16 -> "x16"
        | X17 -> "x17"
        | X18 -> "x18"
        | X19 -> "x19"
        | X20 -> "x20"
        | X21 -> "x21"
        | X22 -> "x22"
        | X23 -> "x23"
        | X24 -> "x24"
        | X25 -> "x25"
        | X26 -> "x26"
        | X27 -> "x27"
        | X28 -> "x28"
        | X29 -> "x29"
        | X30 -> "x30"
        | Sp  -> "sp"
        | Xzr  -> "xzr"

let argumentRegisters = [X0; X1; X2; X3; X4; X5; X6; X7]

let operand rand : string =
    match rand with
        | Cst n     -> string n
        | Reg reg   -> fromReg reg
        | Off8 reg  -> fromReg reg + ", lsl 3"
        | Off16 reg -> fromReg reg + ", lsl 4"

(* Nine registers that can be used for expression evaluation in Arm64 *)

let temporaries =
    [X9; X10; X11; X12; X13; X14; X15; X16; X17]

let mem x xs = List.exists (fun y -> x=y) xs

let getTemp pres : reg64 option =
    let rec aux available =
        match available with
            | []          -> None
            | reg :: rest -> if mem reg pres then aux rest else Some reg
    aux temporaries

(* Get temporary register not in pres; throw exception if none available *)

let getTempFor (pres : reg64 list) : reg64 =
    match getTemp pres with
    | None     -> failwith "no more registers, expression too complex"
    | Some reg -> reg

let pushAndPop reg code = [Push reg] @ code @ [Pop reg]

(* Arm64 instructions are 32 bits, so long constants may require several instructions *)

let loadCst reg (value : int64) : string list = 
    let movk (i : int64) shift =
        if i = 0 then [] else ["movk " + fromReg reg + ", " + string i + ", lsl " + string shift]
    ["mov " + fromReg reg + ", " + string(value &&& 0xffff)]
    @ movk ((value >>> 16) &&& 0xffff) 16
    @ movk ((value >>> 32) &&& 0xffff) 32
    @ movk ((value >>> 48) &&& 0xffff) 48
    
(* Preserve reg across code, on the stack if necessary *)

let preserve reg pres code =
    if mem reg pres then
       pushAndPop reg code
    else
        code

(* Preserve all live registers around code, eg a function call *)

let rec preserveAll pres code =
    match pres with
    | []          -> code
    | reg :: rest -> preserveAll rest (pushAndPop reg code)

(* Generate new distinct labels *)

let (resetLabels, newLabel) = 
    let lastlab = ref -1
    ((fun () -> lastlab.Value <- 0), (fun () -> (lastlab.Value <- 1 + lastlab.Value; "L" + string(lastlab.Value))))

(* Standard Arm64 function postlude *)

let popreturn = [Ins("ldp x29, x30, [sp], 16"); Ins("ret")]

(* Convert one bytecode instr into Arm64 instructions in text form and pass to out *)

let arm64instr2int out instr : unit =
    let outlab lab = out (lab + ":\n")
    let outins ins = out ("\t" + ins + "\n")
    let popReg reg  = outins ("ldr " + fromReg reg + ", [sp], 16")    // Postincrement
    let pushReg reg = outins ("str " + fromReg reg + ", [sp, -16]!")  // Predecrement
    match instr with
      | Arith (ins, rd, rn, op1) -> outins (ins + " " + fromReg rd + ", " + fromReg rn + ", " + operand op1)
      | B lab                    -> outins ("b " + lab)
      | Bl lab                   -> outins ("bl " + lab)            
      | Cbnz (rn, lab)           -> outins ("cbnz " + fromReg rn + ", " + lab)
      | Cbz (rn, lab)            -> outins ("cbz " + fromReg rn + ", " + lab)      
      | Cmp (rn, rm)             -> outins ("cmp " + fromReg rn + ", " + fromReg rm)      
      | Cset (rd, cond)          -> outins ("cset " + fromReg rd + ", " + cond)
      | FLabel (lab, n)          -> (outlab lab;
                                     outins "stp x29, x30, [sp, #-16]!";
                                     outins "mov x29, sp";
                                     List.iter pushReg (List.take n argumentRegisters))
      | Ins ins                  -> outins ins
      | Label lab                -> outlab lab
      | Ldr (rd, rs)             -> outins ("ldr " + fromReg rd + ", [" + fromReg rs + "]")
      | Mov (rd, Cst i)          -> List.iter outins (loadCst rd i)
      | Mov (rd, Reg rs)         -> outins ("mov " + fromReg rd + ", " + fromReg rs)
      | Mov (rd, _)              -> failwith "illegal argument to mov instruction"
      | Msub (rd, rn, rm, ra)    -> outins ("msub " + fromReg rd + ", " + fromReg rn + ", " + fromReg rm + ", " + fromReg ra)
      | Pop r1                   -> popReg r1
      | Push r1                  -> pushReg r1
      | Printi                   -> outins ("bl " + printi)
      | Println                  -> outins ("bl " + println)
      | Str (rs, rd)             -> outins ("str " + fromReg rs + ", [" + fromReg rd + "]")
      
(* Convert instruction list to list of assembly code fragments *)
 
let code2arm64asm (code : arm64 list) : string list =
    let bytecode = ref []
    let outinstr i = (bytecode.Value <- i :: bytecode.Value)
    List.iter (arm64instr2int outinstr) code;
    List.rev (bytecode.Value)

let stdheader =
    ".text\n" +
    ".global " + asm_main + "\n" +
    ".extern " + checkargc + "\n" +
    ".extern " + println + "\n" +
    ".extern " + printi + "\n";

let beforeinit argc =
    asm_main + ":\n" +
    "\tstp x29, x30, [sp, -16]!     \t// Save base pointer and return address\n" +
    "\tmov x29, sp                  \t// Set x29 as base pointer\n" + 
    "\tstp x28, x1, [sp, -16]!      \t// Save x28 and x1 on stack, keep 16-alignment\n" +
    "\tmov x28, sp                  \t// Set x28 as globals base pointer\n" + 
    "\t// Check that Micro-C main and command line argument counts match:\n" +
    "\tmov x1, " + string(argc) + " \t\t\t// The runtime argc is already in x0\n" +
    "\tbl " + checkargc + "\n" +
    "\t// Allocate globals:\n"

let loadmainargs argc =
    let loadReg reg = "\tldr " + fromReg reg + ", [x8], +8"
    "\t// Copy main's arguments from x1[0], x1[1], ... to x0, x1 ...:\n" +
    "\tldr x8, [x28, 8]             \t// Get args array (was x1) from stack\n" +
    String.concat "\n" (List.map loadReg (List.take argc argumentRegisters)) + "\n"

let popglobals =
    "\t// Remove globals from stack and return to C code:\n" +
    "\tmov     sp, x28              \t// Reset stack to globals base\n" +
    "\tldr     x28, [sp], 16        \t// Restore saved x28\n" + 
    String.concat "" (code2arm64asm popreturn) + "\n"
