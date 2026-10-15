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
### ex03.c - Bytcode translation
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
    STI;                         //i = i + 1 
    INCSP -1;                   // remove local variable i

    INCSP 0; 

L3: 
    GETBP; CSTI 1; ADD; LDI;    // get value of i
    GETBP; CSTI 0; ADD; LDI;    // get value of n
    LT;                         // i < n
    IFNZRO "L2";                //loop if LT = not zero

INCSP -1;                       // remove local variable i
RET 0                           // return

```

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

### ex05.c - Bytecode translation

```fsharp
    LDARGS 1;                       // load argument n
    CALL (1, "L1");                 // Call main (L1) with 1 argument
    STOP;

    L1:                             // inside of main
        INCSP 1;                    // make room for outer r;
        GETBP; CSTI 1; ADD;         // address of r
        GETBP; CSTI 0; ADD; LDI;    // load n
        STI;                        // outer r = n
        INCSP -1;                   // discards assignment result
            INCSP 1;                   // add space for inner r
            GETBP; CSTI 0; ADD; LDI;   // load n
            GETBP; CSTI 2; ADD;        // inner &r
            CALL (2, "L2");            // Calling Square with n and &r
            INCSP -1;                  // discards function call
            GETBP; CSTI 2; ADD; LDI;   // load inner r
            PRINTI;                    // print inner r;
            INCSP -1; INCSP -1;        // discard print result
        GETBP; CSTI 1; ADD; LDI;       //load outer r
        PRINTI;                        // print outer r;
        INCSP -1;                      // discard print result
        INCSP -1;                      // remove outer r                
        RET 0;                         // return
    
    L2:
        GETBP; CSTI 1; ADD; LDI;        // *rp
        GETBP; CSTI 0; ADD; LDI;        // i
        GETBP; CSTI 0; ADD; LDI;        // i
        MUL; STI;                       // *rp = i * i
        INCSP -1;                       // discards result of assignment of variable
        INCSP 0;                        // square has no local 
        RET 1                           // return and clean call frame
```

```c
// micro-C example 5 -- return a result via a pointer argument; nested blocks

void main(int n) {
  int r; 
  r = n;
  { 
    int r;
    square(n, &r);
    print r;
  }
  print r;
}

void square(int i, int *rp) {
  *rp = i * i;
}

```
### Execution trace of ex03.c

The execution trace was generated using:

```powershell
java Machinetrace CEx/ex03.out 4 > CEx/ex03trace.txt
```
The trace shows the stack before each instruction is executed.

`LDARGS 1` places the argument `4` on the stack. `CALL` creates the stack frame for `main`, containing the retun address, old bp and paramenter `n`. `INCSP 1` allocates space for local var `i`.

The exact mapping is shown in the previous segment.

Stack trace content
```text
[stack content] {program address: next instruction}
```

## Exercise 8.3
We've extended the `Absyn.fs` file to contain `PreInc` and `PreDec` as well as teh compiler.

in `MicroC\Comp.fs`, we've added the following cases to `cExpr`

```fsharp
    | PreInc acc -> cAccess acc varEnv funEnv @ [DUP; LDI; CSTI 1; ADD; STI]
    | PreDec acc -> cAccess acc varEnv funEnv @ [DUP; LDI; CSTI 1; SUB; STI]
```

For `PreInc`m `cAccess` first calculates the address of the variable. Then using the instructions as specified in the book, we increment the value of the variable by 1.

`PreDec` works in the same way but uses a `SUB` instruction instead of an `ADD` instruction.

``++n` would therefore compile to:
```
address of n
DUP             // Duplicates the address, so it can be used to load and store value.
LDI             // loads current value
CSTI 1          // pushes 1 to stack
ADD             // increments value by 1
STI             // Stores new value on the address of original value address.
```

We've tested the implementation with this program:
```c
void main(int n) {
    print n;
    print ++n;
    print --n;
    print n;
    println;
}
```

We've compiled and run the program with this command:
```
dotnet build microcc.fsproj
dotnet run --project microcc.fsproj -- CEx/ex83.c
java Machine CEx/ex83.out 5
```

The program produces:

```text
5 6 5 5
```

## Exercise 8.4

```c
LDARGS 0;
CALL (0, "L1");
STOP;

Label "L1";
    INCSP 1;
    GETBP; CSTI 0; ADD; CSTI 20000000; // i = 20000000
    STI;                               // save i
    INCSP -1;                          // discard assignment result
    GOTO "L3";
    
Label "L2";
    GETBP; CSTI 0; ADD;        // get addr of i
    GETBP; CSTI 0; ADD; LDI;   // load i
    CSTI 1;                    //
    SUB;                       // sub 1 from i
    STI;                       // save i
    INCSP -1;                  // discard assignment reuslt
    INCSP 0;                   // no local vars

Label "L3";
    GETBP; CSTI 0; ADD; LDI; // get i
    IFNZRO "L2";             // i > 0?
    INCSP -1;
    RET -1

```

Prog1:
```c
0 20000000 70 7 0 1 11 30 72 4 101

(Decoded to instructions)
    CSTI 20000000
    GOTO L2

L1:
    CSTI 1
    SUB

L2:
    DUP
    IFNZRO L1
    STOP
```

As ex08.c has a LOT of extra instructions to run through, compared to prog1, ex08.c will be running a lot slower, as each instruction will take a little bit of time, which will accumulate over 20mil loops runs.

```c
// micro-C example 8 -- loop 20 million times

void main() {
  int i;
  i = 20000000;
  while (i) {
    i = i - 1;
  }
}

```

Code for ex13.c instructions
```c
LDARGS 1;
CALL (1, "L1");
STOP;

Label "L1";
    INCSP 1; GETBP; CSTI 1; ADD;
    CSTI 1889; STI; INCSP -1;
    GOTO "L3";

Label "L2";
    GETBP; CSTI 1; ADD;
    GETBP; CSTI 1; ADD; LDI;
    CSTI 1;
    ADD;
    STI;
    INCSP -1;
    GETBP; CSTI 1; ADD; LDI;
    CSTI 4;
    MOD;
    CSTI 0;
    EQ;
    IFZERO "L7";
    GETBP; CSTI 1; ADD; LDI;
    CSTI 100;
    MOD;
    CSTI 0;
    EQ;
    NOT;
    IFNZRO "L9";
    GETBP; CSTI 1; ADD; LDI;
    CSTI 400;
    MOD;
    CSTI 0;
    EQ;
    GOTO "L8";
    
Label "L9";
    CSTI 1;
    
Label "L8";
    GOTO "L6";

Label "L7";
    CSTI 0;

Label "L6";
    IFZERO "L4"; 
    GETBP; CSTI 1; ADD; LDI;
    PRINTI; INCSP -1;
    GOTO "L5";

Label "L4";
    INCSP 0; 

Label "L5";
    INCSP 0;

Label "L3";
    GETBP; CSTI 1; ADD; LDI;
    GETBP; CSTI 0; ADD; LDI; LT;
    IFNZRO "L2";
    INCSP -1;
    RET 0
```
```c 
// micro-C example 13 -- optimization of andalso and orelse

void main(int n) {
  int y;
  y = 1889;
  while (y < n) {
    y = y + 1;
    if (y % 4 == 0 && (y % 100 != 0 || y % 400 == 0)) 
      print y;
  }
}

```

The code ontains many labels and jumbs because the program combines a `while` loop an `if` statement, `&&` and `||`.

`IFZERO` and `IFNZRO` implements the conditions and short circuit evaluation. `GOTO` is an unconditional jump between different parts of the stack.

`INSCSP 0` does not change the stack. It is unnessesary cleanup code generated for blocks without local variables, showing that the compiler does not optimize redundant instructions.