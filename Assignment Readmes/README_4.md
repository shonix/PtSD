# Assignment 4
All files in this assignment can be found under `.\ProgrammingLanguageConcepts\plcFSharp`

## Exercise 4.5
In the file `\Fun\FunPar.fsi` we've changed the Expr part of the code to recognize AND and OR expressions on line 43 and 44.
In the file `\Fun\FunLex.fsl` we've changed the keyword matcher to match || and && with our newly created AND and OR expressions in the parser.

## Exercise 6.1
Instead of an actual answer for this function:

```fsharp
let add x = let f y = x + y in f end
in add 2 end
```

instead you get a closure missing value of y, since its not supplied as a parameter in the fucnction, and it is not found in the environment.