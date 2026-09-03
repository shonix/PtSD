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
  | If of expr * expr * expr //Exercise 1.1 (iv)

let e1 = CstI 17;;

let e2 = Prim("+", CstI 3, Var "a");;

let e3 = Prim("+", Prim("*", Var "b", CstI 9), Var "a");;


(* Evaluation within an environment *)

let rec eval e (env : (string * int) list) : int =
    match e with
    | CstI i            -> i
    | Var x             -> lookup env x
    | Prim(ope, e1, e2) ->   // Exercise 1.1 (iii)
        let i1 = eval e1 env
        let i2 = eval e2 env
        match ope with
        | "+" -> i1 + i2
        | "-" -> i1 - i2
        | "*" -> i1 * i2
        | "max" -> if i1 > i2 then i1 else i2 //Exercise 1.1 (i)
        | "min" -> if i1 < i2 then i1 else i2 //Exercise 1.1 (i)
        | "=="  -> if i1 = i2 then 1 else 0   //Exercise 1.1 (i)
        | _     -> failwith "unknown Operator"
    | If(e1, e2, e3) -> if eval e1 env <> 0 then eval e2 env else eval e3 env //Exercise 1.1 (v)
    
//Exercise 1.1 (ii)    
let exp1 : expr = Prim("max", (Prim("+", CstI 1, CstI 3)), CstI 5)
let exp2 : expr = Prim("min", Prim("-", CstI 5, Prim("+", CstI 2, CstI 1)), Prim("+", Prim("-", CstI 7, CstI 1), CstI 5))
let exp3 : expr = Prim("==", Prim("+", CstI 12, Prim("-", CstI 5, CstI 2)), CstI 15)
let exp4 : expr = Prim("==", CstI 12, CstI 21)

let MaxT = eval exp1 env //Tests max, should be 5
let MinT = eval exp2 env //Tests min, should be 2
let EvalT1 = eval exp3 env //Tests eval with 2 same numbers
let EvalT2 = eval exp4 env //Tests eval with 2 different numbers

let e1v  = eval e1 env;;
let e2v1 = eval e2 env;;
let e2v2 = eval e2 [("a", 314)];;
let e3v  = eval e3 env;;

//Exercise 1.2 (i)
type aexpr =
    | CstI of int
    | Var of string
    | Add of aexpr * aexpr
    | Sub of aexpr * aexpr
    | Mul of aexpr * aexpr
    
//Exercise 1.2 (ii)
let aexp1 = Sub(Var "v", Add(Var "w", Var "z"))
let aexp2 = Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z")))
let aexp3 = Add (Var "x", Add (Var "y", Add (Var "z", Var "v")))

//Exercise 1.2 (iii)
let rec fmt (e : aexpr) : string =
    match e with
    | CstI i -> string i
    | Var x  -> x
    | Add(e1, e2) -> "(" + fmt e1 + " + " + fmt e2 + ")"
    | Sub(e1, e2) -> "(" + fmt e1 + " - " + fmt e2 + ")"
    | Mul(e1, e2) -> "(" + fmt e1 + " * " + fmt e2 + ")"

let fmtT1 = fmt aexp1
let fmtT2 = fmt aexp2

//Exercise 1.2 (iv)
let rec simplify (e : aexpr) : aexpr =
    match e with
    | CstI i -> CstI i
    | Var x -> Var x
    | Add (e1, CstI 0) -> e1
    | Add (CstI 0, e2) -> e2
    | Sub (e1, CstI 0) -> e1
    | Sub (e1, e2) when e1 = e2 -> CstI 0
    | Mul (_, CstI 0) -> CstI 0
    | Mul (CstI 0, _) -> CstI 0
    | Mul (e1, CstI 1) -> e1
    | Mul (CstI 1, e2) -> e2
    | _ -> e
//Does not take into account simplifying inner expressions such as
//simplify (Mul(Add(CstI 1, CstI 0), Add(Var "x", CstI 0)));;
let SmpT : aexpr = Mul(Add(CstI 1 , CstI 0), Add(Var "x", CstI 0))

//Exercise 1.2 (v)
let rec diffr x (e : aexpr) : aexpr =
    match e with
    | CstI _ -> CstI 0
    | Var v -> if v = x then CstI 1 else CstI 0
    | Add (e1, e2) -> Add(diffr x e1, diffr x e2)
    | Sub (e1, e2) -> Sub(diffr x e1, diffr x e2)
    | Mul (e1, e2) -> Add (Mul (diffr x e1, e2), Mul (e1, diffr x e2)) 

let difT : aexpr = Mul(CstI 18, Var "x")
