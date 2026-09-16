module Exercise3_5
// Run fsi with:
// dotnet fsi -r bin/Debug/net10.0/FsLexYacc.Runtime.dll Absyn.fs ExprPar.fs ExprLex.fs Parse.fs Expr.fs Exercise3_5.fs
open Parse

let tests = [
        "test1",  "2 + 3 * 4"
        "test2",  "1 + 2 * 3"
        "test3",  "1 - 2 - 3"
        "test4",  "1 + -2"
        "test5",  "x+"
        "test6",  "1 + 1.2"
        "test7",  "1 + "
        "test8",  "let z = (17) in z + 2 * 3 end"
        "test9",  "let z = 17) in z + 2 * 3 end"
        "test10", "let in = (17) in z + 2 * 3 end"
        "test11", "1 + let x=5 in let y=7+x in y+y end + x end"
]

let runTest (name, input) =
    try
        let result = fromString input
        printfn "%s: OK" name
        printfn "  Input:  %s" input
        printfn "  Result: %A" result
    with
    | ex ->
        printfn "%s: ERROR" name
        printfn "  Input:   %s" input
        printfn "  Message: %s" ex.Message

let runAll () =
    tests |> List.iter runTest