# Generating and compiling the micro-SQL lexer and parser


## A. Generate and compile the lexer and parser

Generate and compile the lexer and parser by building the `parse.fsproj`
project:

```bash
dotnet build parse.fsproj
```

This will automatically download and install the `fslex` and `fsyacc`
tools, if necessary, and use them to generate files `UsqlLex.fs` and
`UsqlLex.fsi` for the lexer and `UsqlPar.fs` and `UsqlPar.fsi` for the
parser, and also install the `FsLexYacc.Runtime.dll` file.  These
files are used below.

## B. Load micro-SQL lexer and parser

To load the micro-SQL lexer and parser into an interactive F#
session, do this from a command prompt:

```bash
dotnet fsi -r bin/Debug/net10.0/FsLexYacc.Runtime.dll Absyn.fs UsqlPar.fs UsqlLex.fs Parse.fs
```

Now you can exercise the lexer and parser from within the F# session:

```fsharp
open Parse;;
fromString "SELECT Employee.name, salary * (1 - taxrate) FROM Employee";;
```

```fsharp
#q;;
```
