// macfib.s - recursive function Fibonacci in Arm64 assembly for MacOS
// sestoft@itu.dk * 2026-07-27
// Assemble and run with
//   clang -arch arm64 macfib.s -o macfib
//   ./macfib

// Expected result for n=35 is 9227465; for n=45 it is 1134903170. 

.text
.globl _main
.extern _printf

// Entry point
_main:
    stp     x29, x30, [sp, #-16]!       // Save base pointer and return address
    mov     x29, sp

    // Compute fib(n), result in x0
    mov     x0, #35                     // Argument n to fib(n)
    bl      fib

    // Print result using printf("%jd ")
    sub     sp, sp, #16                 // Reserve stack space for printf's 2nd argument 
    str     x0, [sp]                    // Copy x0 to stack top, as printf's 2nd argument
    adrp    x0, printistr@PAGE
    add     x0, x0, printistr@PAGEOFF   // 1st printf argument is format string, to x0
    bl      _printf

    // Print newline using printf("%c", 10)
    mov     x1, #10                     // ASCII newline is character 10
    str     x1, [sp]                    // Copy 10 to stack top, as printf's 2nd argument
    adrp    x0, printcstr@PAGE
    add     x0, x0, printcstr@PAGEOFF   // 1st printf argument is format string, to x0
    bl      _printf
    add     sp, sp, #16

    mov     x0, #0                      // Return 0 from main
    ldp     x29, x30, [sp], #16         // Restore base pointer and return address
    ret

// Recursive Fibonacci: fib(n), argument n in x0, result in x0
fib:
    stp     x29, x30, [sp, #-16]!       // Save base pointer and return address
    mov     x29, sp
    sub     sp, sp, #16                 // Reserve space for n and intermediate result
    str     x0, [x29, -8]               // Save n on stack

    cmp     x0, #2                      // if n < 2 return n
    b.lt    .Lend

    sub     x0, x0, #1                  // n-1
    bl      fib                         // compute fib(n-1), result in x0
    str     x0, [x29, -16]              // save intermediate result fib(n-1)
    ldr     x0, [x29, -8]               // Load n
    sub     x0, x0, #2                  // n-2
    bl      fib                         // compute fib(n-2), result in x0
    ldr     x1, [x29, -16]              // load result of fib(n-1) into x1
    add     x0, x1, x0                  // result = fib(n-1) + fib(n-2)
    b       .Lend

.Lend:
    add     sp, sp, #16                 // Remove local variable space
    ldp     x29, x30, [sp], #16
    ret

.data
printistr:
    .asciz "%jd "
printcstr:
    .asciz "%c"
