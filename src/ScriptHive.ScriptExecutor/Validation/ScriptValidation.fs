namespace ScriptHive.ScriptExecutor.Validation

open System

module public ScriptValidation =
    
    let private forbiddenKeywords = 
        [ "eval"; "Function"; "require"; "fs"; "child_process"; 
          "import"; "fetch"; "XMLHttpRequest"; "while(true)" ]

    let validateContent (content: string) =
        if String.IsNullOrWhiteSpace(content) then
            raise (ArgumentException "Script content cannot be empty.")

        if not (content.Contains("function process(data)")) then
            raise (ArgumentException "Script must contain 'function process(data)'.")

        forbiddenKeywords 
        |> List.iter (fun f ->
            if content.Contains(f) then
                raise (ArgumentException $"Script contains forbidden code: {f}")
        )