module Intro2

open System.Formats.Asn1

//Contains part 1.1 and 1.2

(* Association lists map object language variables to their values *)

let env = [("a", 3); ("c", 78); ("baf", 666); ("b", 111)];;

let emptyenv = []; (* the empty environment *)

let rec lookup env x =
    match env with 
    | []        -> failwith (x + " not found")
    | (y, v)::r -> if x=y then v else lookup r x;;

let cvalue = lookup env "c";;


(* Object language expressions with variables *)

type expr = 
  | CstI of int
  | Var of string
  | Prim of string * expr * expr
  | Brim of string * expr * expr * expr //Keep string such that multiple different boolean expressions can be added

let e1 = CstI 17;;

let e2 = Prim("+", CstI 3, Var "a");;

let e3 = Prim("+", Prim("*", Var "b", CstI 9), Var "a");;


(* Evaluation within an environment *)

let rec eval e (env : (string * int) list) : int =
    match e with
    | CstI i            -> i
    | Var x             -> lookup env x
    | Prim(ope, e1, e2) ->
        let i1 = eval e1 env
        let i2 = eval e2 env
        match ope with
        | "+" -> i1 + i2
        | "-" -> i1 - i2
        | "*" -> i1 * i2
        | "Max" -> if i1 > i2 then i1 else i2
        | "Min" -> if i1 < i2 then i1 else i2
        | "EQ"  -> if i1 = i2 then 1 else 0
        | _     -> failwith "unknown Operator"
    | Brim(ope, e1, e2, e3) -> //follow semantics such that further boolean expressions are easier to add
        let i1 = eval e1 env
        match ope with
        | "if" -> if i1 <> 0 then eval e2 env else eval e3 env
        | _ -> failwith "unknown Brim"
    
let exp1 : expr = Prim("Max", (Prim("+", CstI 1, CstI 3)), CstI 5)
let exp2 : expr = Prim("Min", Prim("-", CstI 5, Prim("+", CstI 2, CstI 1)), Prim("+", Prim("-", CstI 7, CstI 1), CstI 5))
let exp3 : expr = Prim("EQ", Prim("+", CstI 12, Prim("-", CstI 5, CstI 2)), CstI 15)
let exp4 : expr = Prim("EQ", CstI 12, CstI 21)

let MaxT = eval exp1 env //Tests max, should be 5
let MinT = eval exp2 env //Tests min, should be 2
let EvalT1 = eval exp3 env //Tests eval with 2 same numbers
let EvalT2 = eval exp4 env //Tests eval with 2 different numbers

let e1v  = eval e1 env;;
let e2v1 = eval e2 env;;
let e2v2 = eval e2 [("a", 314)];;
let e3v  = eval e3 env;;

type aexpr =
    | CstI of int
    | Var of string
    | Add of aexpr * aexpr
    | Sub of aexpr * aexpr
    | Mul of aexpr * aexpr
    
let aexp1 = Sub(Var "v", Add(Var "w", Var "z"))
let aexp2 = Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z")))

let rec fmt (e : aexpr) : string =
    match e with
    | CstI x -> string x
    | Var s  -> s
    | Add (e1, e2) -> fmt e1 + " + " + fmt e2
    | Sub (e1, e2) -> fmt e1 + " - " + fmt e2
    | Mul (e1, e2) -> fmt e1 + " * " + fmt e2
    
let fmtT1 = fmt aexp1
let fmtT2 = fmt aexp2

let rec simplify (e : aexpr) : aexpr =
    match e with
    | Add (e1, CstI 0) -> e1
    | Add (CstI 0, e2) -> e2
    | Sub (e1, CstI 0) -> e1
    | Sub (e1, e2) when e1 = e2 -> CstI 0
    | Mul (_, CstI 0) -> CstI 0
    | Mul (CstI 0, _) -> CstI 0
    | Mul (e1, CstI 1) -> e1
    | Mul (CstI 1, e2) -> e2
    | Mul (e1, e2) -> simplify (Mul(simplify e1, simplify e2))
    | Sub (e1, e2) -> simplify (Sub(simplify e1, simplify e2))
    | Add (e1, e2) -> simplify (Add(simplify e1, simplify e2))
    | _ -> failwith "Mystic error"

let SmpT : aexpr = Mul(Add(CstI 1 , CstI 0), Add(Var "x", CstI 0))

let rec diffr (e : aexpr) : aexpr =
    match e with
    | CstI _ -> CstI 0
    | Var v -> if v = "x" then CstI 1 else CstI 0
    | Add (e1, e2) -> Add((diffr e1), (diffr e2))
    | Sub (e1, e2) -> Sub((diffr e1), (diffr e2))
    | Mul (Var(x), _) -> Var(x)          
    | Mul (_, Var(x)) -> Var(x)     //I think there are some patterns i dont hit, but lets come back later
    | _ -> failwith "sad error"
    
let difT : aexpr = Mul(CstI 18, Var "x")

