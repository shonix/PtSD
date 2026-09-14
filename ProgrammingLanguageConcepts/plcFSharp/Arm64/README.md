# Compiling and running the micro-C to Arm64 compiler

Chapter 16 describes real machine code in the form of assembly code in
the Arm64 instruction set, and show how to compile micro-C programs to
such code instead of bytecode for our own abstract stack machine.  We
also see that the call stack layouts of both real and abstract
machines are actually very similar.  Executing micro-C programs
translated into real machine code will be much faster, both because it
avoids the interpretive overhead incurred by the abstract machine, and
because it uses machine registers instead of the stack for expression
evaluation.

You need the folder `Arm64`.

To assemble, link and run Arm64 assembly programs, see
`assembly/README`.

## Build the Arm64-generating micro-C compiler and use it

There are three steps

1. Build the micro-C Arm64 compiler.
2. Compile micro-C example programs.
3. Assemble, link and run compiled examples.

### Build the register-based micro-C to Arm64 compiler

```bash
dotnet build
```

### Compile micro-C example programs to Arm64 assembly

Use the command-line compiler:

```bash
dotnet run --project microccarm64.fsproj Arm64Ex/ex11.c 
```

This should output something like:

```bash
Micro-C register-based Arm64 compiler v 0.0.0.1 of 2026-02-16
Compiling Arm64Ex/ex11.c to Arm64Ex/ex11.s
```

### Assemble, link and run the compiled examples

#### On Linux

```bash
clang -Wall -c driver.c
```

```bash
clang ex11.s driver.o -o try11
```

```bash
./try11 8
```

#### On MacOS

```bash
clang -Wall -c driver.c
```

```bash
clang -arch arm64 Arm64Ex/ex11.s driver.o -o try11
```

```bash
./try11 8
```

#### On MS Windows with Arm64

Note: Your Windows computer must have an `Arm64`processor.  Intel
architectures such as `x86`, `x86_64` and `Amd64` will not work with
these micro-C for Arm64 examples.

If you have installed `msys` and `clang` as described in [Platform
Dependencies](../../README.md), you can call `clang` on the command
line (in the `clangarm64` prompt) exactly as shown for Linux above,
and on the same generated assembly files (ex11.s and so on).

