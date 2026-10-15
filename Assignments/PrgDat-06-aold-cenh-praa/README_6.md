# Assignment 6
## Exercise 7.4

We've extended the micro-C abstract syntax with preincrement and predecrement.

In `\MicroC\Absyn.fs`, we've added:

```fsharp
| PreInc of access
| PreDec of access
```

In `\MicroC\Interp.fs`, we've added the following cases to `eval`:

```fsharp
| PreInc acc ->
    let (loc, store1) = access acc locEnv gloEnv store
    let res = getSto store1 loc + 1
    (res, setSto store1 loc res)

| PreDec acc ->
    let (loc, store1) = access acc locEnv gloEnv store
    let res = getSto store1 loc - 1
    (res, setSto store1 loc res)
```

First, `access` finds the location of the variable or array element. We then use `getSto` to read its current value and increment or decrement it. Finally, `setSto` stores the new value at the same location.

The result is returned as a tuple:

```fsharp
(res, setSto store1 loc res)
```

The first part of the tuple is the new value of the expression, and the second part is the updated store.

For example, if `n` contains `5`, evaluating `PreInc(AccVar "n")` returns `6` and changes the stored value of `n` to `6`. Similarly, `PreDec(AccVar "n")` returns and stores `4`.

## Exercise 7.5
We've extended the micro-C lexer and parser to support writing ++ and -- expressions such as `--n` or `++n`.

In `\MicroC\CLex.fsi`, we've added two new lexer rules:

```fsharp
| "++"         { PLUSPLUS }
| "--"         { MINUSMINuS }
```

These rules are placed before the rules for individual `+` and `-` operators.

In `\MicroC\CPar.fsy`, we've added the corresponding tokens:

```fsharp
%token PLUS MINUS PLUSPLUS MINUSMINUS TIMES DIV MOD
```

We've then added the following rules to `ExprNotAccess`:

```fsharp
| PLUSPLUS Access                     { PreInc $2            }
| MINUSMINUS Access                   { PreDec $2            }
```

The parser converts `++n` into:

```fsharp
PreInc(AccVar "n")
```

and `--n` into:

```fsharp
PreDec(AccVar "n")
```

The operand is parsed as an `Access` because the operand must change a stored value. This allows variables, array elemnts and pointers, while rejecting expressions that do not have a writable location.

this is supported then
```c
++n;
--arr[index];
++*pointer;
```

The generated lexer and parser files can be updated by running:

```bash
dotnet build parse.fsproj
```

We've tested the implementation using:

```c
void main(int n) {
    print n;
    print ++n;
    print n;
    print --n;
    print n;
    println;
}
```

when run the with n = 5 the result will be `5 6 6 5 5`

This shows that the operators boh return the ew value and update the value stored in `n`.

## Exercixe 8.1

### ii

```fsharp
LDARGS 1
CALL (1, "L1")
STOP 

L1: 
    INCSP 1                       //int i;

    GETBP; CSTI 1; ADD;          // find address of i
    CSTI 0; STI; INCSP -1;       // i = 0 then // remove local variable i
    
    GOTO "L3";                   // goto while loop
 
L2:
    GETBP; CSTI 1; ADD; LDI;     // find address of i
    PRINTI; INCSP -1;            // print i 
    GETBP; CSTI 1; ADD; 
    GETBP; CSTI 1; ADD; LDI;
    CSTI 1; ADD;
    STI; INCSP -1;              //i = i + 1 then // remove local variable i

    INCSP 0; 

L3: 
    GETBP; CSTI 1; ADD; LDI;    // get value of i
    GETBP; CSTI 0; ADD; LDI;    // get value of n
    LT;                         // i < n
    IFNZRO "L2";                //loop if LT = not zero

INCSP -1;                       // remove local variable i
RET 0                           // return

```

    LDARGS 1                         // load the argument n
    CALL (1, "L1")                   // call main with one argument
    STOP

L1:
    INCSP 1                          // allocate local variable i

    GETBP; CSTI 1; ADD               // calculate address of i
    CSTI 0; STI; INCSP -1            // i = 0; discard assignment result

    GOTO "L3"                        // check condition before first iteration

L2:
    GETBP; CSTI 1; ADD; LDI          // load value of i
    PRINTI; INCSP -1                 // print i; discard print result

    GETBP; CSTI 1; ADD               // calculate address of i
    GETBP; CSTI 1; ADD; LDI          // load value of i
    CSTI 1; ADD                      // calculate i + 1
    STI; INCSP -1                    // store result in i; discard result (i = i + 1)

    INCSP 0                          // leave loop-body block; no locals to remove

L3:
    GETBP; CSTI 1; ADD; LDI          // load value of i
    GETBP; CSTI 0; ADD; LDI          // load value of n
    LT                               // calculate i < n
    IFNZRO "L2"                      // execute body when i < n is true

    INCSP -1                         // remove local variable i
    RET 0                            // return from main

```c
// micro-C example 3

void main(int n) { 
  int i; 
  i=0; 
  while (i < n) { 
    print i; 
    i=i+1;
  } 
}

```