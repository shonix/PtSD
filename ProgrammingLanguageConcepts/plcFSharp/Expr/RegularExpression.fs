module PSDExercises.RegularExpression

//3.3
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