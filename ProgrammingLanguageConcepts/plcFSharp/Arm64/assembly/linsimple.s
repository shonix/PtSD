// linsimple.s - Arm64 assembly program calling libc printf on Linux
// and Windows on Arm (WOA) * sestoft@itu.dk * 2026-07-27

// Assemble, link and run with
//   clang linsimple.s -o linsimple
//   ./linsimple

// This assembly program has the same effect as the C statement
//   printf("The result is ->%jd<-\n", 3456 + 120000);

.global main                         // Entry point of 
.extern printf                       // C library function

.text                                // Code section
main:
  stp x29, x30, [sp, -16]!           // Push x29, x30 on stack
  mov x29, sp                        // Set base pointer = sp
  adrp    x0, mystring               // Set x0=mystring pageaddr
  add     x0, x0, :lo12:mystring     // Add lower 12-bit offset
  mov     x1, 3456                   // Set x1=3456
  mov     x2, (120000 & 0xffff)      // Set x2 low 16 bits 
  movk    x2, (120000 >> 16), lsl 16 // Set x2 upper 16 bits
  add     x1, x1, x2                 // Set x1 = x1 + x2
  bl printf                          // Call printf on x0, x1
  mov x0, 0                          // Return 0 from main
  ldp x29, x30, [sp], 16             // Pop x29, x30
  ret                                // Return to OS

.data                                // Data section
mystring:                            // mystring is here:
  .string "The result is ->%jd<-\n"
