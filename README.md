## Exercise 2.4 (+ 2.5)

See the file `Intcomp1.fs` for implementation of the function `assemble` and `Machine.java` for compiling.


## Exercise 3.2

The regular expression that can recognize all sequences described is $(b|ab)*a?$.

The NFA corresponding to the regular expression is:
![](/Users/nch/RiderProjects/PtSD/image/NFA.png)

The DFA corresponding to the NFA is below:
![](/Users/nch/RiderProjects/PtSD/image/DFA.png)

## Exercise 3.3
The rightmost derivation of the string: `let z = (17) in z + 2 * 3 end EOF`:
```
// let z = (17) in z + 2 * 3 end EOF
//Main                                                          :Rule A
//Expr EOF                                                      :Rule B
//LET NAME EQ Expr IN Expr END EOF                              :Rule F
//LET NAME EQ Expr IN Expr PLUS Expr END EOF                    :Rule H
//LET NAME EQ Expr IN Expr PLUS Expr TIMES Expr END EOF         :Rule G
//LET NAME EQ Expr IN Expr PLUS Expr TIMES CSTINT(3) END EOF    :Rule C
//LET NAME EQ Expr IN Expr PLUS CSTINT(2) TIMES 3 END EOF       :Rule C
//LET NAME EQ Expr IN NAME PLUS 2 TIMES 3 END EOF               :Rule B
//LET NAME EQ LPAR Expr RPAR IN z PLUS 2 TIMES 3 END EOF        :Rule E
//LET NAME EQ (CSTINT(17)) IN z PLUS 2 TIMES 3 END EOF          :Rule C
//LET z EQ (17) IN z PLUS 2 TIMES 3 END EOF                     :Rule B

// -> let z = (17) IN z + 2 * 3 END EOF
```

## Exercise 3.4
Drawing of the above derivation as a tree:
![](/Users/nch/RiderProjects/PtSD/image/TREE.png)