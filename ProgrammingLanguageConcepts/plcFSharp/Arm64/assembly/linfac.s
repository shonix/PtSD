// linfac.s - recursive function (factorial) in 64-bit Arm64 assembly
// for Linux and Windows on Arm (WOA) * sestoft@itu.dk * 2026-07-27

// Assemble and run with
//   clang linfac.s -o linfac
//   ./linfac
//
// Expected result is 2432902008176640000


.text
.globl main
.extern printf

// Entry point
main:
    stp     x29, x30, [sp, #-16]!       // Save base pointer and return address
    mov     x29, sp

    // Compute fac(20), result in x0
    mov     x0, #20                     // Argument n = 20
    bl      fac 

    // Print result using printf("%jd ")
    mov     x1, x0                      // Copy x0 to x1, as printf's 2nd argument
    adrp    x0, printistr
    add     x0, x0, :lo12:printistr     // 1st printf argument is format string, to x0
    bl      printf

    // Print newline using printf("%c", 10)
    mov     x1, #10                     // ASCII 10 is newline, printf's 2nd argument
    adrp    x0, printcstr
    add     x0, x0, :lo12:printcstr     // 1st printf argument is format string, to x0
    bl      printf

    mov     x0, #0                      // Return 0 from main
    ldp     x29, x30, [sp], #16         // Restore base pointer and return address
    ret

// Recursive factorial: fac(n), argument n in x0, result in x0
fac:
    stp     x29, x30, [sp, #-16]!       // Save base pointer and return address
    mov     x29, sp
    sub     sp, sp, #16                 // Reserve space for local variable n (8 bytes)
    str     x0, [x29, -16]              // Save n on stack

    cmp     x0, #0                      // if n == 0
    b.eq    .Lbase

    sub     x0, x0, #1                  // n-1
    bl      fac                         // fac(n-1), result in x0
    ldr     x1, [x29, -16]              // Load original n
    mul     x0, x0, x1                  // result *= n
    b       .Lend

.Lbase:
    mov     x0, #1                      // return 1

.Lend:
    add     sp, sp, #16                 // Remove local variable space
    ldp     x29, x30, [sp], #16
    ret

.data
printistr:
    .asciz "%jd "
printcstr:
    .asciz "%c"
