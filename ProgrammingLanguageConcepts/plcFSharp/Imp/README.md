# Compiling and loading the naive imperative evaluator

Chapter 7 discusses imperative programming languages, in which the
value of a variable may be modified destructively by assignment.  We
first present a naive imperative language where a variable denotes an
updatable store cell, and then present the environment/store model
used in real imperative programming languages.  Then we introduce
micro-C, a C-style imperative language, and show how to execute it
using an interpreter We present the concepts of expression, variable
declaration, assignment, loop, output, variable scope, lvalue and
rvalue, parameter passing mechanisms, pointer, array, and pointer
arithmetics.

## A. Loading and running the naive imperative language

```bash
dotnet fsi Naive.fs
```

```fsharp
open Naive;;
run ex1;;
```

```fsharp
run ex2;;
#q;;
```
   
## B. Compiling ```array.c```

Use clang C compiler to compile ```array.c``` on your platform. E.g., on MacOS:

```bash
clang array.c
```

and then execute the result program in a terminal. E.g., on MacOS:

```bash
./a.out
11 22
22 22
11 22
```

## C. To run Parameters example:

```bash
cd Parameters
dotnet build Parameters.csproj
dotnet run 11 22
```
