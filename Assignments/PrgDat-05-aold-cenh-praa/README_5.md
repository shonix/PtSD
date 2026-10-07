# Assignment 5
## Exercise 6.4
### (i)
The hard to spot $e_1$ in the top right corner reads:
```
e_1 = if x < 10 then 42 else f(x-1)
```
![Alt text](20261006_144807.jpg)

### (ii)
If we have a value with no type, when we've completed the truth tree, the tree must be polymorphic.

Since every type is infered/known in our tree, it must be monomorphic.

## Exercise 6.5
For each of the functions here are the results we've found.

**ex01**  
`let f x = 1 in f f end`  
Type is an `int`.

**ex02**  
`let f g = g g in f end`  
Type could not be infered.

g g is not typable because g must simultaniously be a function AND an argument to itself. This produces a circular type equation `'a` = `'a` -> `'b` where no finite type exists.

The equation `'a` = `'a` -> `'b` cannot be solved using ordinary finite types because defining 'a requires 'a to already appear inside its own definition.

**ex03**  
`let f x = let g y = y in g false end in f 42 end`  
Type is a `bool`.

**ex04**  
`let f x = let g y = if true then y else x in g false end in f 42 end`  
Type could not be infered.  
Expected `bool`, but received `int`.

`g` is called with false, so y has type `bool`. both branches of the `if` statement must have the same type, so `x` needs to be a `bool`. Therefore, `f` gas type `bool -> bool` but is called with an `int` which is `42` which results in an error.

**ex05**  
`let f x = let g y = if true then y else x in g false end in f true end`
Type is a `bool`.

### (ii)

`bool -> bool`
let f x =   
if x then true else false   
in f end

`int -> int`  
let f x = x + 1  
in f end

`int -> int  -> int`  
let f x y = x + y  
in f end

`'a -> 'b -> 'a`   
let f x g = x  
in f end  

`'a -> 'b -> 'b`  
let f x g = g  
in f end

`('a -> b') ('b -> 'c) ('a -> 'c)`  
let compose f g x =  
    g (f x)  
in compose end

```fsharp
//This block is a more intricate but more explicit way to write the previous statement.
let compose f g x = g (f x)
in
    let addOne x = x + 1
    in
        let double x = x * 2
        in compose addOne double 3
        end
    end
end
```

This is some weird stuff.. because of nontermination, if i create a function which recurses forever, then it can technically take any type, and promise to return any other type. Because it never actually returns anything...  
`'a -> 'b`  
let f x = f x  
in f end  

okay so now we give the function a parameter 0. which is never used, sure, but we now know the input. but since we have an unbound variable which previously was 'b, because 'a has been freed up, 'a is now used as the return type. again, weird stuff...  
`'a`  
let f x = f x  
in f 0 end  