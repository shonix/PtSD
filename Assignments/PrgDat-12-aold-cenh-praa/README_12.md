# Assignment 13
## Exercise 1.1
We've run the commands `dotnet run -verbose JavaEx/ex01.java` and `dotnet run -verbose JavaEx/ex01.java`

We've done this for both ex01 and ex02

Output for the command `dotnet run -verbose JavaEx/ex01.java`
```fs
VERBOSE: System Information
  Framework: .NET 10.0.12
  Environment.Version: 10.0.12
  OS: Microsoft Windows 10.0.26200
  OS Acrhitecture: X64
Compiling JavaEx/ex01.java to JavaEx/ex01.out.


VERBOSE: Parsing file JavaEx/ex01.java

VERBOSE: Program after parsing: 
class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b = new B();
    b.i = 43;
    A ba;
    ba = b;
    ba.i = 42;
    println(ba.i,b.i);
  }
}

VERBOSE: Program after Return and Reachability Analysis: 
class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b = new B();
    b.i = 43;
    A ba;
    ba = b;
    ba.i = 42;
    println(ba.i,b.i);
    return;
  }
}

VERBOSE: Class hierarchy: 
Object <- A <- B
       <- Main


VERBOSE: Program as class hierarchy: 


class Object extends  {
}

class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b = new B();
    b.i = 43;
    A ba;
    ba = b;
    ba.i = 42;
    println(ba.i,b.i);
    return;
  }
}


VERBOSE: Class Table for Main: 
(main, ()): () -> void

Class Table for B: 

i: int
i: int
Class Table for A: 

i: int
Class Table for Object: 



VERBOSE: Program with types as class hierarchy: 


class Object extends  {
}

class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b(B) = new B()(B);
    b(B).i(int) = 43(int);
    A ba;
    ba(A) = b(B);
    ba(A).i(int) = 42(int);
    println(ba(A).i(int),b(B).i(int))(int);
    return;
  }
}


VERBOSE: Program after A-normalization for GC: 

class Object extends  {
}

class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b(B) = new B()(B);
    b(B).i(int) = 43(int);
    A ba;
    ba(A) = b(B);
    ba(A).i(int) = 42(int);
    println(ba(A).i(int),b(B).i(int))(int);
    return;
  }
}

VERBOSE: Compiling file JavaEx/ex01.java

VERBOSE: Class Environment: 
  A (L2_class_A): Object:
  Field Environment with next offset 2:
    A.i:int -> 1
  vTable Environment with next offset 0:

  B (L3_class_B): A:
  Field Environment with next offset 3:
    A.i:int -> 1
    B.i:int -> 2
  vTable Environment with next offset 0:

  Main (L5_class_Main): Object:
  Field Environment with next offset 1:

  vTable Environment with next offset 1:
    (main, ()) -> (0,L4_main)
  Object (L1_class_Object): :
  Field Environment with next offset 1:

  vTable Environment with next offset 0:
```
Output for `dotnet run -debug JavaEx/ex01.java `
```fs
VERBOSE: System Information
  Framework: .NET 10.0.12
  Environment.Version: 10.0.12
  OS: Microsoft Windows 10.0.26200
  OS Acrhitecture: X64
Compiling JavaEx/ex01.java to JavaEx/ex01.out.


VERBOSE: Parsing file JavaEx/ex01.java

VERBOSE: Program after parsing: 
class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b = new B();
    b.i = 43;
    A ba;
    ba = b;
    ba.i = 42;
    println(ba.i,b.i);
  }
}

VERBOSE: Program after Return and Reachability Analysis: 
class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b = new B();
    b.i = 43;
    A ba;
    ba = b;
    ba.i = 42;
    println(ba.i,b.i);
    return;
  }
}

VERBOSE: Class hierarchy: 
Object <- A <- B
       <- Main


VERBOSE: Program as class hierarchy: 


class Object extends  {
}

class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b = new B();
    b.i = 43;
    A ba;
    ba = b;
    ba.i = 42;
    println(ba.i,b.i);
    return;
  }
}


VERBOSE: Class Table for Main: 
(main, ()): () -> void

Class Table for B: 

i: int
i: int
Class Table for A: 

i: int
Class Table for Object: 



VERBOSE: Program with types as class hierarchy: 


class Object extends  {
}

class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b(B) = new B()(B);
    b(B).i(int) = 43(int);
    A ba;
    ba(A) = b(B);
    ba(A).i(int) = 42(int);
    println(ba(A).i(int),b(B).i(int))(int);
    return;
  }
}


VERBOSE: Program after A-normalization for GC: 

class Object extends  {
}

class A extends Object {
  int i;
}

class B extends A {
  int i;
}

class Main extends Object {
  void main() { 
    B b;
    b(B) = new B()(B);
    b(B).i(int) = 43(int);
    A ba;
    ba(A) = b(B);
    ba(A).i(int) = 42(int);
    println(ba(A).i(int),b(B).i(int))(int);
    return;
  }
}

VERBOSE: Compiling file JavaEx/ex01.java

VERBOSE: Class Environment: 
  A (L2_class_A): Object:
  Field Environment with next offset 2:
    A.i:int -> 1
  vTable Environment with next offset 0:

  B (L3_class_B): A:
  Field Environment with next offset 3:
    A.i:int -> 1
    B.i:int -> 2
  vTable Environment with next offset 0:

  Main (L5_class_Main): Object:
  Field Environment with next offset 1:

  vTable Environment with next offset 1:
    (main, ()) -> (0,L4_main)
  Object (L1_class_Object): :
  Field Environment with next offset 1:

  vTable Environment with next offset 0:


VERBOSE: Compiled program has 74 bytecode instructions.

PS C:\Users\peter\Documents\ITU\5. Semester\Programmer som data\PtSD\Assignments\PrgDat-12-aold-cenh-praa\MicroJ> dotnet run -debug JavaEx/ex01.java  
Micro-Java compiler v 1.00 of 2026-05-14
Compiling JavaEx/ex01.java to JavaEx/ex01.out.


DEBUG Program AST after parsing: Program
  [Classdec
     ("A", "Object", [Fielddec (TypI, "i", { line = 9
                                             column = 14 })], { line = 9
                                                                column = 6 });
   Classdec ("B", "A", [Fielddec (TypI, "i", { line = 10
                                               column = 24 })], { line = 10
                                                                  column = 6 });
   Classdec
     ("Main", "Object",
      [Methoddec
         (TypV, "main", [],
          Block
            ([Dec (TypO "B", "b", { line = 14
                                    column = 6 });
              Stmt
                (Expr
                   (Assign
                      (AccVar ("b", None, { line = 14
                                            column = 6 }),
                       New ("B", None, { line = 14
                                         column = 10 }), { line = 14
                                                           column = 10 })));
              Stmt
                (Expr
                   (Assign
                      (AccFld
                         (Access (AccVar ("b", None, { line = 15
                                                       column = 4 })), "i", None,
                          { line = 15
                            column = 4 }), Cst (CstI 43, None, { line = 15
                                                                 column = 10 }),
                       { line = 15
                         column = 4 }))); Dec (TypO "A", "ba", { line = 16
                                                                 column = 6 });
              Stmt
                (Expr
                   (Assign
                      (AccVar ("ba", None, { line = 16
                                             column = 6 }),
                       Access (AccVar ("b", None, { line = 16
                                                    column = 11 })),
                       { line = 16
                         column = 11 })));
              Stmt
                (Expr
                   (Assign
                      (AccFld
                         (Access (AccVar ("ba", None, { line = 17
                                                        column = 4 })), "i",
                          None, { line = 17
                                  column = 4 }),
                       Cst (CstI 42, None, { line = 17
                                             column = 11 }), { line = 17
                                                               column = 4 })));
              Stmt
                (Expr
                   (PrimC
                      ("println",
                       [Access
                          (AccFld
                             (Access (AccVar ("ba", None, { line = 19
                                                            column = 12 })), "i",
                              None, { line = 19
                                      column = 12 }));
                        Access
                          (AccFld
                             (Access (AccVar ("b", None, { line = 19
                                                           column = 18 })), "i",
                              None, { line = 19
                                      column = 18 }))], None, { line = 19
                                                                column = 4 })))],
             { line = 14
               column = 4 }), { line = 13
                                column = 7 })], { line = 12
                                                  column = 6 })]

DEBUG Program AST after Return and Reachability Analysis: Program
  [Classdec
     ("A", "Object", [Fielddec (TypI, "i", { line = 9
                                             column = 14 })], { line = 9
                                                                column = 6 });
   Classdec ("B", "A", [Fielddec (TypI, "i", { line = 10
                                               column = 24 })], { line = 10
                                                                  column = 6 });
   Classdec
     ("Main", "Object",
      [Methoddec
         (TypV, "main", [],
          Block
            ([Dec (TypO "B", "b", { line = 14
                                    column = 6 });
              Stmt
                (Expr
                   (Assign
                      (AccVar ("b", None, { line = 14
                                            column = 6 }),
                       New ("B", None, { line = 14
                                         column = 10 }), { line = 14
                                                           column = 10 })));
              Stmt
                (Expr
                   (Assign
                      (AccFld
                         (Access (AccVar ("b", None, { line = 15
                                                       column = 4 })), "i", None,
                          { line = 15
                            column = 4 }), Cst (CstI 43, None, { line = 15
                                                                 column = 10 }),
                       { line = 15
                         column = 4 }))); Dec (TypO "A", "ba", { line = 16
                                                                 column = 6 });
              Stmt
                (Expr
                   (Assign
                      (AccVar ("ba", None, { line = 16
                                             column = 6 }),
                       Access (AccVar ("b", None, { line = 16
                                                    column = 11 })),
                       { line = 16
                         column = 11 })));
              Stmt
                (Expr
                   (Assign
                      (AccFld
                         (Access (AccVar ("ba", None, { line = 17
                                                        column = 4 })), "i",
                          None, { line = 17
                                  column = 4 }),
                       Cst (CstI 42, None, { line = 17
                                             column = 11 }), { line = 17
                                                               column = 4 })));
              Stmt
                (Expr
                   (PrimC
                      ("println",
                       [Access
                          (AccFld
                             (Access (AccVar ("ba", None, { line = 19
                                                            column = 12 })), "i",
                              None, { line = 19
                                      column = 12 }));
                        Access
                          (AccFld
                             (Access (AccVar ("b", None, { line = 19
                                                           column = 18 })), "i",
                              None, { line = 19
                                      column = 18 }))], None, { line = 19
                                                                column = 4 })));
              Stmt (Return (None, { line = 0
                                    column = 0 }))], { line = 14
                                                       column = 4 }),
          { line = 13
            column = 7 })], { line = 12
                              column = 6 })]

DEBUG Class hierarchy value: 
Hierarchy
  (Classdec ("Object", "", [], { line = 0
                                 column = 0 }),
   [Hierarchy
      (Classdec
         ("A", "Object", [Fielddec (TypI, "i", { line = 9
                                                 column = 14 })], { line = 9
                                                                    column = 6 }),
       [Hierarchy
          (Classdec
             ("B", "A", [Fielddec (TypI, "i", { line = 10
                                                column = 24 })], { line = 10
                                                                   column = 6 }),
           [])]);
    Hierarchy
      (Classdec
         ("Main", "Object",
          [Methoddec
             (TypV, "main", [],
              Block
                ([Dec (TypO "B", "b", { line = 14
                                        column = 6 });
                  Stmt
                    (Expr
                       (Assign
                          (AccVar ("b", None, { line = 14
                                                column = 6 }),
                           New ("B", None, { line = 14
                                             column = 10 }), { line = 14
                                                               column = 10 })));
                  Stmt
                    (Expr
                       (Assign
                          (AccFld
                             (Access (AccVar ("b", None, { line = 15
                                                           column = 4 })), "i",
                              None, { line = 15
                                      column = 4 }),
                           Cst (CstI 43, None, { line = 15
                                                 column = 10 }), { line = 15
                                                                   column = 4 })));
                  Dec (TypO "A", "ba", { line = 16
                                         column = 6 });
                  Stmt
                    (Expr
                       (Assign
                          (AccVar ("ba", None, { line = 16
                                                 column = 6 }),
                           Access (AccVar ("b", None, { line = 16
                                                        column = 11 })),
                           { line = 16
                             column = 11 })));
                  Stmt
                    (Expr
                       (Assign
                          (AccFld
                             (Access (AccVar ("ba", None, { line = 17
                                                            column = 4 })), "i",
                              None, { line = 17
                                      column = 4 }),
                           Cst (CstI 42, None, { line = 17
                                                 column = 11 }), { line = 17
                                                                   column = 4 })));
                  Stmt
                    (Expr
                       (PrimC
                          ("println",
                           [Access
                              (AccFld
                                 (Access (AccVar ("ba", None, { line = 19
                                                                column = 12 })),
                                  "i", None, { line = 19
                                               column = 12 }));
                            Access
                              (AccFld
                                 (Access (AccVar ("b", None, { line = 19
                                                               column = 18 })),
                                  "i", None, { line = 19
                                               column = 18 }))], None,
                           { line = 19
                             column = 4 })));
                  Stmt (Return (None, { line = 0
                                        column = 0 }))], { line = 14
                                                           column = 4 }),
              { line = 13
                column = 7 })], { line = 12
                                  column = 6 }), [])])


DEBUG Program AST with types as class hierarchy: 
Hierarchy
  (Classdec ("Object", "", [], { line = 0
                                 column = 0 }),
   [Hierarchy
      (Classdec
         ("A", "Object", [Fielddec (TypI, "i", { line = 9
                                                 column = 14 })], { line = 9
                                                                    column = 6 }),
       [Hierarchy
          (Classdec
             ("B", "A", [Fielddec (TypI, "i", { line = 10
                                                column = 24 })], { line = 10
                                                                   column = 6 }),
           [])]);
    Hierarchy
      (Classdec
         ("Main", "Object",
          [Methoddec
             (TypV, "main", [],
              Block
                ([Dec (TypO "B", "b", { line = 14
                                        column = 6 });
                  Stmt
                    (Expr
                       (Assign
                          (AccVar ("b", Some (TypO "B"), { line = 14
                                                           column = 6 }),
                           New ("B", Some (TypO "B"), { line = 14
                                                        column = 10 }),
                           { line = 14
                             column = 10 })));
                  Stmt
                    (Expr
                       (Assign
                          (AccFld
                             (Access
                                (AccVar ("b", Some (TypO "B"), { line = 15
                                                                 column = 4 })),
                              "i", Some TypI, { line = 15
                                                column = 4 }),
                           Cst (CstI 43, Some TypI, { line = 15
                                                      column = 10 }),
                           { line = 15
                             column = 4 })));
                  Dec (TypO "A", "ba", { line = 16
                                         column = 6 });
                  Stmt
                    (Expr
                       (Assign
                          (AccVar ("ba", Some (TypO "A"), { line = 16
                                                            column = 6 }),
                           Access
                             (AccVar ("b", Some (TypO "B"), { line = 16
                                                              column = 11 })),
                           { line = 16
                             column = 11 })));
                  Stmt
                    (Expr
                       (Assign
                          (AccFld
                             (Access
                                (AccVar ("ba", Some (TypO "A"), { line = 17
                                                                  column = 4 })),
                              "i", Some TypI, { line = 17
                                                column = 4 }),
                           Cst (CstI 42, Some TypI, { line = 17
                                                      column = 11 }),
                           { line = 17
                             column = 4 })));
                  Stmt
                    (Expr
                       (PrimC
                          ("println",
                           [Access
                              (AccFld
                                 (Access
                                    (AccVar
                                       ("ba", Some (TypO "A"), { line = 19
                                                                 column = 12 })),
                                  "i", Some TypI, { line = 19
                                                    column = 12 }));
                            Access
                              (AccFld
                                 (Access
                                    (AccVar
                                       ("b", Some (TypO "B"), { line = 19
                                                                column = 18 })),
                                  "i", Some TypI, { line = 19
                                                    column = 18 }))], Some TypI,
                           { line = 19
                             column = 4 })));
                  Stmt (Return (None, { line = 0
                                        column = 0 }))], { line = 14
                                                           column = 4 }),
              { line = 13
                column = 7 })], { line = 12
                                  column = 6 }), [])])


DEBUG Program AST after A-normalization for GC: Hierarchy
  (Classdec ("Object", "", [], { line = 0
                                 column = 0 }),
   [Hierarchy
      (Classdec
         ("A", "Object", [Fielddec (TypI, "i", { line = 9
                                                 column = 14 })], { line = 9
                                                                    column = 6 }),
       [Hierarchy
          (Classdec
             ("B", "A", [Fielddec (TypI, "i", { line = 10
                                                column = 24 })], { line = 10
                                                                   column = 6 }),
           [])]);
    Hierarchy
      (Classdec
         ("Main", "Object",
          [Methoddec
             (TypV, "main", [],
              Block
                ([Dec (TypO "B", "b", { line = 14
                                        column = 6 });
                  Stmt
                    (Expr
                       (Assign
                          (AccVar ("b", Some (TypO "B"), { line = 14
                                                           column = 6 }),
                           New ("B", Some (TypO "B"), { line = 14
                                                        column = 10 }),
                           { line = 14
                             column = 10 })));
                  Stmt
                    (Expr
                       (Assign
                          (AccFld
                             (Access
                                (AccVar ("b", Some (TypO "B"), { line = 15
                                                                 column = 4 })),
                              "i", Some TypI, { line = 15
                                                column = 4 }),
                           Cst (CstI 43, Some TypI, { line = 15
                                                      column = 10 }),
                           { line = 15
                             column = 4 })));
                  Dec (TypO "A", "ba", { line = 16
                                         column = 6 });
                  Stmt
                    (Expr
                       (Assign
                          (AccVar ("ba", Some (TypO "A"), { line = 16
                                                            column = 6 }),
                           Access
                             (AccVar ("b", Some (TypO "B"), { line = 16
                                                              column = 11 })),
                           { line = 16
                             column = 11 })));
                  Stmt
                    (Expr
                       (Assign
                          (AccFld
                             (Access
                                (AccVar ("ba", Some (TypO "A"), { line = 17
                                                                  column = 4 })),
                              "i", Some TypI, { line = 17
                                                column = 4 }),
                           Cst (CstI 42, Some TypI, { line = 17
                                                      column = 11 }),
                           { line = 17
                             column = 4 })));
                  Stmt
                    (Expr
                       (PrimC
                          ("println",
                           [Access
                              (AccFld
                                 (Access
                                    (AccVar
                                       ("ba", Some (TypO "A"), { line = 19
                                                                 column = 12 })),
                                  "i", Some TypI, { line = 19
                                                    column = 12 }));
                            Access
                              (AccFld
                                 (Access
                                    (AccVar
                                       ("b", Some (TypO "B"), { line = 19
                                                                column = 18 })),
                                  "i", Some TypI, { line = 19
                                                    column = 18 }))], Some TypI,
                           { line = 19
                             column = 4 })));
                  Stmt (Return (None, { line = 0
                                        column = 0 }))], { line = 14
                                                           column = 4 }),
              { line = 13
                column = 7 })], { line = 12
                                  column = 6 }), [])])

DEBUG Main method with label L4_main found in class L5_class_Main with vTable index 0

DEBUG Bytecode: 
     0: PUSHLAB L5_class_Main
     2: HEAPALLOC 4 1
     5: HEAPCOPY 1
     7: LDARGS 0
     9: VCALL 1 0
    12: STOP
LABEL L5_class_Main
    13: LabelAddr L4_main
LABEL L4_main
    14: NIL
    15: GETBP
    16: CSTI 1
    18: ADD
    19: STACKADDR
    20: PUSHLAB L3_class_B
    22: CSTI 0
    24: CSTI 0
    26: HEAPALLOC 4 3
    29: HEAPCOPY 3
    31: STD
    32: INCSP -1
    34: GETBP
    35: CSTI 1
    37: ADD
    38: STACKADDR
    39: LDD
    40: CSTI 2
    42: HEAPADDR
    43: CSTI 43
    45: STD
    46: INCSP -1
    48: NIL
    49: GETBP
    50: CSTI 2
    52: ADD
    53: STACKADDR
    54: GETBP
    55: CSTI 1
    57: ADD
    58: STACKADDR
    59: LDD
    60: STD
    61: INCSP -1
    63: GETBP
    64: CSTI 2
    66: ADD
    67: STACKADDR
    68: LDD
    69: CSTI 1
    71: HEAPADDR
    72: CSTI 42
    74: STD
    75: INCSP -1
    77: GETBP
    78: CSTI 2
    80: ADD
    81: STACKADDR
    82: LDD
    83: CSTI 1
    85: HEAPADDR
    86: LDD
    87: PRINTI
    88: INCSP -1
    90: GETBP
    91: CSTI 1
    93: ADD
    94: STACKADDR
    95: LDD
    96: CSTI 2
    98: HEAPADDR
    99: LDD
   100: PRINTI
   101: PRINTNL
   102: INCSP -1
   104: RET 2
   106: INCSP -2
LABEL L3_class_B
LABEL L2_class_A
LABEL L1_class_Object

DEBUG Machine.code2insts.LabEnv: [("L1_class_Object", 108); ("L2_class_A", 108); ("L3_class_B", 108);
 ("L4_main", 14); ("L5_class_Main", 13)]
```


## Exercise 1.2
1. True
2. False
3. True 
4. False
5. True
6. False
7. False
8. True
9. Flase
10. False
11. False

## Exercise 1.4
1. The 2nd signature is the most specific as it matches 1 to 1 with the actual method signature
2. The 4th signature is the most specific as it matches 1 to 1 with the actual method signature
3. the 2nd signature is the most specific, as its the only applicable one seeing as U is NOT a subclass of V, so it can't be the 3rd specific signature.
4. Because there is no most specific signature for these method signatures, because both the 3.rd and 4.th signatures can be used by the actual method signature.
5. Because null is a subtype of every class, there can be no most specific signature type unless there is a (f, null, null null) or something like that.
6. It's the most abstract, so it cannot use any of the signatures.
7. Because the is no signature with (f, Object, S, U (or object)) then this method signature does not have a most specific signature.