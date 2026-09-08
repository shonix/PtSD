module PSDExercises.RegularExpression

    //regular expression checker (actual logic)
let regular (inp : char list) : bool =
    let rec aux (lst : char list) (last : bool) : bool =
        match lst with
        | [] -> true        //List is empty and we've not hit a false so it must be regular
        | x :: xs when x = 'a' -> if last then false         //letter is a and so was our last
                                          else aux xs true   //letter is a and last letter was b
        | _ :: xs              -> aux xs false               //letter is b son it does not really matter
    aux inp false
    
    //eat string and call regular with a char list (what to call :D)
let isRegular (inp : string) : bool =
    regular (List.ofSeq inp)