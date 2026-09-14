# Micro-C Arm64 Test Suite

Micro-C Arm64 includes an automated test suite located in:

```text
Arm64Ex/test.fsx
```

The test suite consists of mirco-C example programs also found in the
`MicroC/CEx` folder.

Run the test suite from the `Arm64Ex` directory:

```text
% dotnet fsi test.fsx 
Compiling file ex01.c

10 9 8 7 6 5 4 3 2 1 

Used 0.009 s

Compiling file ex03.c

0 1 2 3 4 5 6 7 8 9 
Used 0.006 s
...
Programs that succeed.
ex01.c: OK
ex03.c: OK
ex04.c: OK
...
ex27.c: OK
ex28.c: OK
% 
```

Some programs are not included in the test suite, because they output
unpredicable physical addresses: `ex02.c` and `ex23.c`.

The test script:

- compiles and runs all test programs,
- executes the generated executables.
- compares each execution with the expected output found in the file `test.fsx`
- summarizes the results at the end,
- works across multiple platforms.

> **Note:** The test suite runs relatively slowly because it launches
    external system processes to compile and execute the programs.
