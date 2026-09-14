# PtSD

Assignment work and textbook sources, versioned together in shonix/PtSD.
The textbook sources retain their original LICENSE and README in ProgrammingLanguageConcepts.

## Assignment locations

| Work | Location under ProgrammingLanguageConcepts/plcFSharp |
| --- | --- |
| Original introductory example | Intro/Intro1.fs |
| Assignment 1 F# exercises | Intro/Intro2.fs |
| Exercise 1 Java exercise | Intro/Assignment1Java/ |
| Assignment 2 interpreter/compiler | Intcomp/Intcomp1.fs |
| Assignment 2 Java stack machine | Intcomp/Machine.java |
| Exercise 3.3 notes | Expr/RegularExpression.fs |

Open PSDExercises.sln for the F# project. It now references the files above:

```powershell
dotnet build PSDExercises.sln
dotnet fsi ProgrammingLanguageConcepts/plcFSharp/Intro/Intro2.fs
dotnet fsi ProgrammingLanguageConcepts/plcFSharp/Intcomp/Intcomp1.fs
```
```powershell
cd ProgrammingLanguageConcepts/plcFSharp/Intro/Assignment1Java
javac --enable-preview --release 23 *.java
java --enable-preview Main
```

This reorganization preserves the existing exercise implementations. The
Intcomp F# code has incomplete-pattern warnings for multi-binding Let expressions.
Intcomp/Machine.java still needs its readBytecode method implemented before it
can compile. These are assignment-code issues, not missing files from the move.
