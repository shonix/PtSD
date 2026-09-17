4.1, 4.2, 4.3, 4.4
# Assignment 3
All pieces of code added has been marked with comments, throughtout the specified folders/files.

## Exercise 3.*
All files regarding these exercises can be found under the `ProgrammingLanguageConcepts\plcFSharp\Expr\` folder.

## Exercise 3.5
See the file `Exercise3_5.fs` for implementation of a test suite, that tests all the exercise test cases.

## Exercise 3.6
See the files `Parse.fs` and `Expr.fs` for implementation of scomp. The actual implementation is in `Parse.fs` but uses functions from `Expr.fs`

## Exercise 3.7
`Absyn.fs` has been modified to to include `If` for conditional expressions. `ExprLex.fsl` and `ExprPar.fsy` has also been modified to support IF statements.

## Exercise 4.*
All files regarding these exercises can be found under the `ProgrammingLanguageConcepts\plcFSharp\Fun\` folder.

## Exercise 4.1
The Readme was followed. not much output to show for it.

## Exercise 4.2
See the file `ParseAndRun.fs` for all the exercise programs. Run them with 
```bash
dotnet build
```

```bash
dotnet fsi -r bin/Debug/net10.0/FsLexYacc.Runtime.dll Absyn.fs FunPar.fs FunLex.fs Parse.fs Fun.fs ParseAndRun.fs
```

```fsharp
open ParseAndRun;;
ex1;;
ex2;;
ex3;;
ex4;;
ex5;;
ex6;;
```

## Exercise 4.3
We have modified several places:
`Absyn.fs`:
```fsharp
    | Letfun of string * string list * expr * expr
    | Call of expr * expr list
```
We made Let fun take a list of strings as param names. And we made Call take a list of expressions as arguments.

In `Fun.fs` we've edited the big eval function like so:

```fsharp
type value = 
  | Int of int
  | Closure of string * string list * expr * value env

//Rest of eval function above
      | Call(Var f, eArg) -> 
      let fClosure = lookup env f
      match fClosure with
      | Closure (f, x, fBody, fDeclEnv) ->
//Since xVal is not evaluating to a single value anymore, but to a list of values, we iterate over, and map over each value of the eArg list, and save them as the new xVal
        let xVal = List.map(fun e -> Int(eval e env)) eArg 
// Then we bind the values of the arguments given, to each arguments name, taken from the list of name supplied in "x" in the Closure line above
        let bindings = List.zip x xVal 
// Then finally, we append our zipped tuple of (name, value) list to our function, appended to the environmnet.
        let fBodyEnv = bindings @ (f, fClosure) :: fDeclEnv
        eval fBody fBodyEnv

```

## Exercise 4.4

we changed `Letfun` and `Call` to take lists, so functions can have multiple parameters and calls can have multiple arguments.

In `FunPar.fsy`, we added:

```fsharp
%type <Absyn.expr list> Arguments
%type <string list> Params
```

we then changed function declarations to use Params:  
```
| LET NAME Params EQ Expr IN Expr END { Letfun($2, $3, $5, $7) }
```

Then we added parser rules that collect arguments and parameters into lists:
```
Arguments:
    AtExpr { [$1] }
  | AtExpr Arguments { $1 :: $2 }
;

Params:
    NAME { [$1] }
  | NAME Params { $1 :: $2 }
;
```
This makes syntax like let add x y = x + y in add 3 4 end possible.