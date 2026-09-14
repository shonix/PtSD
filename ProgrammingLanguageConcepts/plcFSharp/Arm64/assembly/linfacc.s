// linfacc.s - in Arm64 assembly for Linux and Windows on Arm (WOA).
// Shows how to call an assembly-defined recursive function fac from
// C code, and how to call C functions from assembly.
// sestoft@itu.dk * 2026-07-27
 
// Compile, assemble, link and run with
//   clang -c driver.c
//   clang linfacc.s driver.o -o linfacc
//   ./linfacc 20

// Expected result is
// 1 
// 1 
// 2 
// 6 
// 24 
// 120 
// 720 
// 5040 
// 40320 
// 362880 
// 3628800 
// 39916800 
// 479001600 
// 6227020800 
// 87178291200 
// 1307674368000 
// 20922789888000 
// 355687428096000 
// 6402373705728000 
// 121645100408832000 
// 2432902008176640000 

.text
.global asm_main
.extern checkargc
.extern printc    
.extern printi

asm_main:
    // x0 is argc, x1 is reference to long array holding command line arguments
    stp     x29, x30, [sp, #-16]!       // Save base pointer and return address
    mov     x29, sp                     // Set x29 as base pointer
    str     x28, [sp, #-16]!            // Save old x28 on stack, keep 16-alignment
    mov     x28, sp                     // Set x28 as globals base pointer
        
    sub     sp, sp, 16                  // Allocate space for globals n and i
    ldr     x1, [x1]
    str     x1, [x28, -8]               // store argument n
    str     xzr, [x28]                  // store counter i, initially 0

    mov     x1, 1                       // check that we have exactly 1 argument
    bl      checkargc

.main_loop:
    ldr     x0, [x28]                   // i
    ldr     x1, [x28, -8]               // n
    cmp     x0, x1
    b.gt     .main_end                  // return if i > n
    
    bl      fac                         // Compute fac(i), result in x0
    bl      printi                     // Print result from x0
    mov     x0, 10                      
    bl      printc                     // Print newline (ASCII 10) using printc

    ldr     x0, [x28]                   // i++
    add     x0, x0, 1
    str     x0, [x28]
    b       .main_loop

.main_end: 
    mov     sp, x28                     // Reset stack to globals base
    ldr     x28, [sp], 16               // Pop and restore saved x28
    ldp     x29, x30, [sp], 16          // Restore base pointer and return address
    ret

// Recursive factorial: fac(m), argument m in x0, result in x0
fac:
    stp     x29, x30, [sp, -16]!        // Save base pointer and return address
    mov     x29, sp
    sub     sp, sp,  16                 // Reserve space for local variable m (8 bytes)
    str     x0, [x29, -16]              // Save m on stack

    cmp     x0, 0                       // if m == 0
    b.eq    .Lbase

    sub     x0, x0, 1                   // m-1
    bl      fac                         // fac(m-1), result in x0
    ldr     x1, [x29, -16]              // Load original m
    mul     x0, x0, x1                  // result *= m
    b       .Lend

.Lbase:
    mov     x0, 1                       // return 1

.Lend:
    add     sp, sp, #16                 // Remove local variable space
    ldp     x29, x30, [sp], #16
    ret
