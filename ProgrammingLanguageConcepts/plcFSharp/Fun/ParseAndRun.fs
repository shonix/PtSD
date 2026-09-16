(* File Fun/ParseAndRun.fs *)

module ParseAndRun

let fromString = Parse.fromString;;

let eval = Fun.eval;;

let run e = eval e [];;

// Exercise 4.1 and 4.2
let ex1 = run (fromString "let y = 7 in y + 2 end");;
let ex2 = run (fromString "let f x = x + 7 in f 2 end");;
let ex3 = run (fromString "let sum n = if n = 0 then 0 else n + (sum (n - 1)) in sum 1000 end")
let ex4 = run (fromString "let pow n = if n = 0 then 1 else 3 * (pow (n - 1)) in pow 8 end")
let ex5 =
    run (
        fromString
            "let powsum n =
                if n = 0 then 1
                else
                    (let pow y =
                        if y = 0 then 1
                        else 3 * (pow (y - 1))
                     in pow n end)
                    + (powsum (n - 1))
             in powsum 11 end"
    )

let ex6 =
    run (
        fromString
            "let powsum n =
                if n = 1 then 1
                else
                    (let pow y =
                        if y = 0 then 1
                        else n * (pow (y - 1))
                     in pow 8 end)
                    + (powsum (n - 1))
             in powsum 10 end"
    )

    // how ex5 works
    // pow y = 
    //     match y with
    //     | 0 -> 1
    //     | y -> 3 * pow (y - 1)

    // powsum n =
    //     match n with
    //     | 0 -> 1
    //     | n -> pow n + powsum (n - 1)
    