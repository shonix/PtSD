# Assignment 8
## Exercise 10.1
### (i)
#### ADD 
Untags the top two stack values, adds them, tags the result, stores it in the lower stack position and then decreases `sp` (the stack pointer).
#### CSTI i
Tags the constant `i`, pushes it onto the stack, and increases stackpointer
#### NIL
pushes the untagged null reference `0`. this is different from `CSTI 0` because it pushes the tagged integer zero to the stack, represented by `1`.
#### IFZERO
This pops the top value on the stack, and checks wheather it is integer zero or nil. It then jumps to a given address if it is zero.
#### CONS
Allocates a two-field heap block, stores the top two stack values in it, and preplaces them with a pointer to the new block.
#### CAR
Replaces the con-cell pointer on top of the stack with tis first field. It reports an error if the pointer is nil.
#### SETCAR
stores the top stack value in the first field of the cons cell below it. the value is popped, but the cons pointer remains.
### (ii)
A block header contains a tag, a length and two color bits.

`Length(header)` shifts the header two bits to the right to remove the color bits. it then applies a mask to remove the tag bits. The result is the block's number of fields, excluding the header.

`Color(header)` performs a bitwise AND and `3` whose binary representation is `11`. This extracts the final two color bits from the header.

`Paint(header, color)` clears the final two color bits, and replaces them with the supplied color. It leaves the block's tag and length unchanged

### (iii)
For List-C, the instruction loop calls `allocate` when it executes `CONS` because a new cons cell requires a heap block. in the unified MicroVM, `HEAPALLOC` can also call `allocate` for other heap-allocated objects.

The stack and its current stack pointer are passed to `allocate`. If garbage collection is needed, they provide the collector's root set. Because this mark-sweep collector does not move live blocks, their addresses do not need to be updated afterwards

### (iv)
`collect` is called when `allocate` searches the entire freelist without finding a free block large enough for the requested allocation.

After collection, `allocate` searches the freelist again. If it still cannot find a sufficiently large block, the machine reports `out of memory`. This can happen because the heap is full or because the available memory is split into blocks that are too small.

## Exercise 10.2
For exersice 10.2 we've made changes in `microvm.c`.

We've added a new method, `mark` and made changes in `markPhase` and `sweepPhase`.

`mark` is a helper function, which uses BFS to go through all reachable blocks in the heap, and marks them with the color `Black`.

`mark` is a recursive function, and returns if it hits a block which aren't white (because then this block has been marked.)

In `markPhase` we find the root of every block, which then utulizes `mark` to mark all the children of a block.

`sweepPhase` then goes through every block in the heap, and the for each block

- Black: reachable, so keeps it and repaints it White for the next collection
- White: Unreachable, so paint it Blue and add it to the freelist
- Blue: already free or an orphan, so leaves it alone.

then it becomes something like this:

```
Before marking: live and dead allocated blocks are White
After marking:  reachable = Black, unreachable = While
After sweeping: reachable = White, unreachable = Blue/free
```
## Exercise 10.3
We've now made changes to sweepPhase so that two adjacent dead blocks become one larger free block.