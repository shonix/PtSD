# PtSD
## File structure
We have a file structure where you can find the answers to exercises in this was:
Exercise 1.1, 1.2 in the file `PSDFsharpfiles\Assignment1.fs`
Exercise 2.1, 2.2 and 2.3 in the file `PSDFsharpfiles\Assignment1_part2.fs`
Exercise 1.4 in the folder `JavaFiles\`

## Exercise 1.1
Extended eval with match cases matching "min" "max" and "==" for PRIM and If matchcase. Made sure evaluation for If was handled correctly. Such that if e1 = 0 then e2 else e3

Extended the Type Expr with:
`| If of expr * expr * expr //Exercise 1.1 (iv)`

We also added some examples called: exp1 - exp4.

## Exercise 1.2
We have added a new Aexpr data type with the given constructors per exercise description. This being CstI, Var, Add, Mul, Sub, for constants, variables, addition, multiplication, and subtraction.

We've added representation for the expressions in aexp1- aexp3:
```
Sub(Var "v", Add(Var "w", Var "z"))
Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z")))
Add (Var "x", Add (Var "y", Add (Var "z", Var "v")))
```

Added function fmt and simplify and diffr (differentation).
In Simplify we've covered the cases in the given table, but have not extended to cover simplifying inner expressions.

For diffr we used regular math for differentiating using the product rule, for multiplying two functions with eachother and differentiating them.

## Exercise 1.4
We've ported the logic to Java, and the files can be found as such:
```
Add.java    Child of Binop
Binop.java  Child of Expr
Csti.java   Child of Expr
Expr.java   Abstract Class
Main.java   Entrance to program
Mul.java    Child of Binop
Sub.java    Child of Binop
Var.java    Child of Expr
```

In Add, Sub and Mul toString has been implemented recursively by overriding JAVAs toString function.
Eval has also been implemented to these classes using basic JAVA math functions such as `e1.eval(env) + e2.eval(env)` in Add.java
This, as is apparent, is also done recursively.

Our 3 (or more) expressions are located in Main.java and evaluates to the same as they would in our F# project.

## Exercise 2.1
We have changed Let to be of the form `string * expr * expr` to `(string * expr) list * expr` as described in the exercise.

To evaluate this list of tuples in Eval, we've added a new `| Let(bind, exp) ->` match case, which adds our expression to a temp env list. Then we evaluate the final expression, using the given env as well as the newly added env1 which contains unevaluated expressions bound to variables, which will be evaluated in the final expr evaluation.

## Exercise 2.2 & 2.3
We've changed the `| Let(x, erhs, ebody) -> ` to `| Let([x, erhs], ebody) -> ` 
So changing the match case so it matches the sequential let-binding constructor from the `Type expr`\
Catching all errors when environment is not as expected with `_ -> failswith "msg` has also been added.