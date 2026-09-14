// macsimple.s - Arm64 assembly program calling libc printf on MacOS
// sestoft@itu.dk * 2026-07-27

// Assemble and run with
//   clang -arch arm64 macsimple.s -o macsimple
//   ./macsimple

// This assembly program has the same effect as the C statement
//    printf("The result is ->%jd<-\n", 3456 + 120000);

// This MacOS version differs from Linux linsimple.c in these ways:
// - External names must be preceded by an underscore (_main, _printf)
// - The page and page offset addresses are obtained by @PAGE and @PAGEOFF
// - The second argument to _printf must be given on the stack, not in x1

.global _main                        // Entry point of this code
.extern _printf                      // Reference to C library function

.text                                // Code section
_main:
  stp x29, x30, [sp, -16]!           // Push x29, x30 on stack
  mov x29, sp                        // Set base pointer = sp
  adrp    x0, mystring@PAGE          // Set x0=mystring pageaddr
  add     x0, x0, mystring@PAGEOFF   // Add mystring page offset
  mov     x1, 3456                   // Set x1=3456
  mov     x2, (120000 & 0xffff)      // Set x2 low 16 bits
  movk    x2, (120000 >> 16), lsl 16 // Set x2 upper 16 bits
  add     x1, x1, x2                 // Set x1 = x1 + x2
  str     x1, [sp, -16]!             // Push x1 as printf 2nd arg
  bl _printf                         // Call printf on x0 and x1
  add     sp, sp, 16                 // Remove printf 2nd arg
  mov     x0, 0                      // Return 0 from main
  ldp     x29, x30, [sp], 16         // Pop x29, x30 
  ret                                // Return to OS

.data                                // Data section
mystring:                            // mystring is here:
  .string "The result is ->%jd<-\n"
