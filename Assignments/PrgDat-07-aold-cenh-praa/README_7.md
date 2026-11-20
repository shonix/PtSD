# Assignment 7
## Exercise 8.5
We've added conditional expressions in the form:

```c
condition ? value1 : value2
```

In `Absyn.fs` we've added:

```fsharp
| Cond of expr * expr * expr
```

In `CLex.fsl` we've added tokens for `?` and `:`:

```fsharp
| '?' -> QUESTION
| ':' -> COLON
```

In `CPar.fsy` we've added:

```fsharp
| Expr QUESTION Expr COLON Expr { Cond($1, $3, $5) }
```

Then in `Comp.fs` we've added:

```fsharp
| Cond(e1, e2, e3) ->
    let labElse = newLabel()
    let labEnd = newLabel()

    cExpr e1 varEnv funEnv
    @ [IFZERO labElse]
    @ cExpr e2 varEnv funEnv
    @ [GOTO labEnd; Label labElse]
    @ cExpr e3 varEnv funEnv
    @ [Label labEnd]
```

It first checks the condition and then only evaluates one of the two expressions. The result of whichever expression is used is left on the stack.

## Exercise 8.6

We've added a `switch` statement.

In `Absyn.fs` we've added:

```fsharp
| Switch of expr * (int * stmt) list
```

The expression is the value being checked, and the list contains the case numbers together with their statement blocks.

In `CLex.fsl` we've added the two keywords:

```fsharp
| "switch" -> SWITCH
| "case"   -> CASE
```

In `CPar.fsy` we've added a rule for `switch`:

```fsharp
| SWITCH LPAR Expr RPAR LBRACE Cases RBRACE { Switch($3, $6) }
```

Cases are parsed into a list:

```fsharp
Cases:
    /* empty */                         { []                 }
  | CASE CaseValue COLON Block Cases    { ($2, $4) :: $5     }
;

CaseValue:
    CSTINT                              { $1                  }
  | MINUS CSTINT                        { -$2                 }
;
```

In `Comp.fs`, each case is compared with the switch value. If they do not match, it jumps to the next case. If they match, the case body runs and then jumps to the end so we do not get fall-through.

Since `switch` is a statement and does not return a value, we've added its compiler implementation to `cStmt` instead of `cExpr`.

```fsharp
| Switch(e, cases) ->
    let labEnd = newLabel()

    let rec compileCases cases =
        match cases with
        | [] ->[]
        | [(caseValue, body)] ->
            [DUP; CSTI caseValue; EQ; IFZERO labEnd]
            @ cStmt body varEnv funEnv

        | (caseValue, body) :: rest ->
            let labNext = newLabel()

            [DUP; CSTI caseValue; EQ; IFZERO labNext]
            @ cStmt body varEnv funEnv
            @ [GOTO labEnd; Label labNext]
            @ compileCases rest

    cExpr e varEnv funEnv
    @ compileCases cases
    @ [Label labEnd; INCSP -1]
```

First, `cExpr e` evaluates the expression inside `switch` and leaves its value on the stack.

For each case, `DUP` copies the switch value so we still have the original value after comparing it. `CSTI caseValue` pushes the value of the current case, and `EQ` checks if they are equal.

```text
DUP
CSTI caseValue
EQ
```

If they are not equal, `IFZERO labNext` jumps to the next case:

```fsharp
[IFZERO labNext]
```

If they are equal, the body is compiled using:

```fsharp
cStmt body varEnv funEnv
```

After running a matching body, `GOTO labEnd` jumps past the remaining cases. This prevents the fall-through behavior found in normal C switch statements.

```fsharp
[GOTO labEnd; Label labNext]
```

The last case is handled separately because it does not need a new label for another case. If it matches, its body runs and execution naturally continues to the end. If it does not match, it jumps directly to `labEnd`.

Finally:

```fsharp
[Label labEnd; INCSP -1]
```

marks the end of the entire switch and removes the original switch value from the stack.

So it basically works by keeping the switch value on the stack, comparing a copy with every case and jumping either to the next case or to the end.

We tested it using `CEx/ex86.c`:

```c
void main(int n) {
    switch (n) {
        case 1: {
            print 111;
        }
        case 2: {
            print 222;
        }
        case 3: {
            print 333;
        }
    }

    println;
}
```

It prints `111`, `222` or `333` depending on the argument. Giving it another number just prints an empty line. This also shows that the cases do not fall through into each other.

## Exercise 9.1
```
local variables:
0 -> i
1 -> least
2 -> tmp
3 -> j


==Setup outer loop==
IL_0000:  ldc.i4.0			//Push 0 to stack
IL_0001:  stloc.0			//Bind 0 to i
IL_0002:  br.s       IL_0032		//Jump to outer loop condition

==Start of outer loop==
IL_0004:  ldloc.0			//Push value of 0 (i) to stack
IL_0005:  stloc.1			//Pop i into value least (1)

==Inner loop setup==
IL_0006:  ldloc.0			//Push i to stack
IL_0007:  ldc.i4.1			//push 1 to stack
IL_0008:  add				//Pops both values and pushes result i+1 = 1
IL_0009:  stloc.3			//Bind result to j
IL_000a:  br.s       IL_001a		//Jump to inner loop condition

==Inner loop body==
IL_000c:  ldarg.1			//load array onto stack
IL_000d:  ldloc.3			//load j onto stack
IL_000e:  ldelem.i4			//Pop array and j and push arr[j] to stack (if no exceptions occur)
IL_000f:  ldarg.1			//push array onto stack again
IL_0010:  ldloc.1			//push least onto stack
IL_0011:  ldelem.i4			//pop top two and push arr[least] onto stack
IL_0012:  bge.s      IL_0016		//pop entire stack and jump to 0016 if arr[j] arr[least]
--Only reached if j < least
IL_0014:  ldloc.3			//push j to stack
IL_0015:  stloc.1			//record new least

==Inner loop increment==
IL_0016:  ldloc.3			//Push j onto stack
IL_0017:  ldc.i4.1			//push 1 onto stack
IL_0018:  add				//add 1 to j
IL_0019:  stloc.3			//bind result to j

==Inner loop condition==
IL_001a:  ldloc.3			//push j to stack
IL_001b:  ldarg.1			//push arr to stack
IL_001c:  ldlen				//replace arr with length of arr on stack
IL_001d:  conv.i4			//convert length to a 32 bit int
IL_001e:  blt.s      IL_000c		//compare and jump to inner loop if j < len

==int tmp = arr[i];== ( dont know what to call these :O )
IL_0020:  ldarg.1			//push arr to stack
IL_0021:  ldloc.0			//load i to stack
IL_0022:  ldelem.i4			//pop top 2 and load arr[i] to stack
IL_0023:  stloc.2			//save as tmp

==arr[i] = arr[least];==
IL_0024:  ldarg.1			//push arr to stack
IL_0025:  ldloc.0			//load i to stack
IL_0026:  ldarg.1			//load arr to stack
IL_0027:  ldloc.1			//load least to stack
IL_0028:  ldelem.i4			//read arr[least]
IL_0029:  stelem.i4			//pops value and stores it into arr[i]

==arr[least] = tmp;==
IL_002a:  ldarg.1			//push arr to stack
IL_002b:  ldloc.1			//push least to stack
IL_002c:  ldloc.2			//push tmp to stack
IL_002d:  stelem.i4			//write tmp into arr[least]

==Outer loop increment==
IL_002e:  ldloc.0			//load i to stack
IL_002f:  ldc.i4.1			//load 1 to stack
IL_0030:  add				//add 1 to i
IL_0031:  stloc.0			//bind result to i

==Outer loop condition==
IL_0032:  ldloc.0			//load i to stack
IL_0033:  ldarg.1			//load arr to stack
IL_0034:  ldlen				//replace arr with length of arr
IL_0035:  conv.i4			//convert len to 32 bit int
IL_0036:  blt.s      IL_0004		//jump to start of outer loop if i < len

IL_0038:  ret				//Return
```

## Exercise 9.3

With the broken version you can see the probram runs out of memory after 3 seconds:
```sh
[2.913s][info][gc] GC(34) Concurrent Mark Cycle 340.391ms
[2.914s][info][gc] GC(36) Pause Young (Normal) (G1 Evacuation Pause) 63M->63M(64M) 0.604ms
[3.081s][info][gc] GC(37) Pause Full (G1 Compaction Pause) 63M->63M(64M) 167.614ms
[3.286s][info][gc] GC(38) Pause Full (G1 Compaction Pause) 63M->63M(64M) 204.237ms
[3.288s][info][gc] GC(39) Pause Young (Concurrent Start) (G1 Evacuation Pause) 63M->63M(64M) 0.673ms
[3.288s][info][gc] GC(41) Concurrent Mark Cycle
[3.291s][info][gc] GC(40) Pause Full (G1 Compaction Pause) 63M->0M(8M) 2.767ms
[3.291s][info][gc] GC(41) Concurrent Mark Cycle 3.145ms
Exception in thread "main" java.lang.OutOfMemoryError: Java heap space
        at QueueWithMistake.runThreads(QueueWithMistake.java:58)
        at QueueWithMistake.main(QueueWithMistake.java:28)
```

Wheras the fixed queue runs without problem for atleast 1½ minutes.
```sh
[93.508s][info][gc] GC(675) Pause Young (Normal) (G1 Evacuation Pause) 38M->1M(64M) 1.048ms
[93.655s][info][gc] GC(676) Pause Young (Normal) (G1 Evacuation Pause) 38M->1M(64M) 0.816ms
[93.799s][info][gc] GC(677) Pause Young (Normal) (G1 Evacuation Pause) 38M->1M(64M) 0.849ms
[93.941s][info][gc] GC(678) Pause Young (Normal) (G1 Evacuation Pause) 38M->1M(64M) 0.813ms
[94.082s][info][gc] GC(679) Pause Young (Normal) (G1 Evacuation Pause) 38M->1M(64M) 0.957ms
[94.222s][info][gc] GC(680) Pause Young (Normal) (G1 Evacuation Pause) 38M->1M(64M) 0.755ms
[94.375s][info][gc] GC(681) Pause Young (Normal) (G1 Evacuation Pause) 38M->1M(64M) 0.985ms
```

So by adding just the line where we set `first.next = null;` we make sure we dont keep references by non used nodes alive, and they're able to be garbage collected. 

    if ( match ( buf ) ) {
        haha = 1;
    }
}