# Assignment 4
All files in this assignment can be found under `.\ProgrammingLanguageConcepts\plcFSharp`

## Exercise 4.5
In the file `\Fun\FunPar.fsi` we've changed the Expr part of the code to recognize AND and OR expressions on line 43 and 44.
In the file `\Fun\FunLex.fsl` we've changed the keyword matcher to match || and && with our newly created AND and OR expressions in the parser.

## Exercise 5.7
In the file `\TypedFun\TypedFun.fs` we've added 
```fsharp
  | TypL of typ  (* list, element type is typ   *) // Ex 5.7
```
 as described in the book, and extended `type tyexpr` 
 ```fsharp
 | List of tyexpr list * typ
 ```

 and the `let rec typ` evaluator is extended with
 ```fsharp
    | List(l, t) -> //Ex. 5.7 forall is great and it checks all types is same type thank you and please.
      if List.forall(fun elem -> typ elem env = t) l 
      then t 
      else failwith "List: List types differ" 
 ```

 which checks if all the elements in a list, is the same type as the provided type `t` when `typ` is called.


## Exercise 6.1
Instead of an actual answer for this function:

```fsharp
let add x = let f y = x + y in f end
in add 2 end
```

instead you get a closure missing value of y, since its not supplied as a parameter in the fucnction, and it is not found in the environment.

## Exercise 6.2
We've added several functions to support the use of anonymous functions.

In the file `\Fun2\Absyn.fs` we've added the line `| Fun of string * expr` to support functions without names, taking one parameter.

In the file `\Fun2\HigherFun.fs` we've added several lines. 

We've added the clos type to support a different closure, when the function has no name.
```fsharp
type value = 
  | Int of int
  | Closure of string * string * expr * value env       (* (f, x, fBody, fDeclEnv) *)
  | Clos of string * expr * value env (* 6.2 *)

```
Then we've added the matchcases to support both the `fun` keyword as well as the `Clos` type

```fsharp
...
| Fun(x, eBody) -> Clos(x, eBody, env)
    | Call(eFun, eArg) -> 
      let fClosure = eval eFun env  
      match fClosure with
      | Clos(x, ebody, fDeclEnv) ->
        let xVal = eval eArg env
        let fBodyEnv = (x, xVal) :: fDeclEnv
        in eval ebody fBodyEnv
...
```
Here we've added the evaluation functionality to evaluate first a fundtion without a name, and then in the Call, a Clos case, which evaluates the argument, appends the given variable in the function declaration environment, and hten evaluates the body of the function with the fbodyenv as per usual.

## Exercise 6.3
We've added several lines to the program to support parsing/lexing anonymous functions.

In `\Fun2\FunLex.fsl` we've added the keyword `|"fun" -> FUN` which let us look for the keyword "fun" while lexing a string. Then we've added the "->" token which parses as "ARR" under the rule token sections, to lets us read the `->` symbol as the token ARR.

In `\Fun2\FunPar.fsy` we've added IN and ARR as tokens, and then set ARR as having almost the lowest precedens, and made sure it evaluates from right to left.

In Expr: we've added this line `| FUN NAME ARR Expr { Fun($2, $4)}` which lets us parse an anonymous function in the form as seen. And then we call the `eval` function in `HigherFun.fs` when a structure matching this statement is found.