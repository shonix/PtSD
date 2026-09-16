(* File Expr/Parse.fs *)
(* Lexing and parsing of simple expressions using fslex and fsyacc *)
(*dotnet fsi -r bin/Debug/net10.0/FsLexYacc.Runtime.dll Absyn.fs ExprPar.fs ExprLex.fs Expr.fs Parse.fs Exercise3_5.fs*)

module Parse

open System
open System.IO
open System.Text
open FSharp.Text
open Absyn
open Expr

(* Plain parsing from a string, with poor error reporting *)

let fromString (str : string) : expr =
    let lexbuf = Lexing.LexBuffer<char>.FromString(str)
    try 
      ExprPar.Main ExprLex.Token lexbuf
    with 
      | exn -> let pos = lexbuf.EndPos 
               failwithf "%s near line %d, column %d\n" 
                  (exn.Message) (pos.Line+1) pos.Column
             
(* Parsing from a text file *)

let fromFile (filename : string) : expr =
    use reader = new StreamReader(filename)
    let lexbuf = Lexing.LexBuffer<char>.FromTextReader reader
    try 
      ExprPar.Main ExprLex.Token lexbuf
    with 
      | exn -> let pos = lexbuf.EndPos 
               failwithf "%s in file %s near line %d, column %d\n" 
                  (exn.Message) filename (pos.Line+1) pos.Column

// Assignment 3.6
let compString (str : string) : sinstr list = 
  fromString str |> fun expr -> scomp expr [] // By using already created functions we convert a string, 
                                                           // to an expression, which we then convert to list of machine instructions.

// Example Assignment 3.5
let ex1 = fromString "2 + 3 * 4"
// Example Assignment 3.7
let ex2 = compString "if 1 then 2 else 3"
