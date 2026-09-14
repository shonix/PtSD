(* File Arm64/MicroCCArm64.fs *)

module MicroCCArm64

let args = System.Environment.GetCommandLineArgs();;

let _ = printf "Micro-C register-based Arm64 compiler v 0.0.0.1 of 2026-02-16\n";;

let _ = 
   if args.Length > 1 then
      let source = args.[1]
      let stem = if source.EndsWith(".c")
                 then source.Substring(0,source.Length-2) 
                 else source
      let target = stem + ".s"
      printf "Compiling %s to %s\n" source target;
      try ignore (Arm64Comp.compileToFile (Parse.fromFile source) source target)
      with Failure msg -> printf "ERROR: %s\n" msg
   else
      printf "Usage: dotnet run --project microccarm64.fsproj <source file>\n";;
