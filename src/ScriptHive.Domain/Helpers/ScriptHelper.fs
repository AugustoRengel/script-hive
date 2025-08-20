namespace ScriptHive.Domain.Helpers

open System
open System.Text.Json
open Jint
open Jint.Native

module ScriptHelper =
    let private forbiddenKeywords = 
        [ "eval"; "Function"; "require"; "fs"; "child_process"; 
          "import"; "fetch"; "XMLHttpRequest"; "while(true)" ]

    let private validateContent(content: string) =
        if String.IsNullOrWhiteSpace(content) then
            raise (ArgumentException "Script content cannot be empty.")

        if not (content.Contains("function process(data)")) then
            raise (ArgumentException "Script must contain 'function process(data)'.")

        forbiddenKeywords |> List.iter (fun f ->
            if content.Contains(f) then
                raise (ArgumentException $"Script contains forbidden code: {f}")
        )

    let rec jsonElementToJsValue (engine: Engine) (je: JsonElement) : JsValue =
        match je.ValueKind with
        | JsonValueKind.Object ->
            let dictObj =
                je.EnumerateObject()
                |> Seq.map (fun prop -> prop.Name, jsonElementToJsValue engine prop.Value)
                |> dict
            JsValue.FromObject(engine, dictObj)
        | JsonValueKind.Array ->
            let arr = je.EnumerateArray() |> Seq.map (jsonElementToJsValue engine) |> Seq.toArray
            JsValue.FromObject(engine, arr)
        | JsonValueKind.String -> JsValue.FromObject(engine, je.GetString())
        | JsonValueKind.Number ->
            match je.TryGetInt64() with
            | true, v -> JsValue.FromObject(engine, v)
            | false, _ -> JsValue.FromObject(engine, je.GetDouble())
        | JsonValueKind.True -> JsValue.FromObject(engine, true)
        | JsonValueKind.False -> JsValue.FromObject(engine, false)
        | JsonValueKind.Null -> JsValue.Null
        | _ -> raise (ArgumentException $"Unsupported JSON kind: {je.ValueKind}")

    let private executeWithTestData(processScript: string) (inputJson: string) (expectedOutput: string): bool =
        let engine =
            new Engine(fun options ->
                options.LimitMemory(4_000_000) |> ignore
                options.TimeoutInterval(TimeSpan.FromSeconds(4.0)) |> ignore
                options.MaxStatements(1000) |> ignore
            )

        [ "console"; "require"; "process"; "fs"; "fetch"; "XMLHttpRequest" ]
        |> List.iter (fun name -> engine.SetValue(name, JsValue.Undefined) |> ignore)
        
        let doc = JsonDocument.Parse(inputJson)
        let jsInput = jsonElementToJsValue engine doc.RootElement

        // Carrega o script
        try engine.Execute(processScript) |> ignore
        with ex -> raise (ArgumentException $"Error executing script: {ex.Message}")

        let processFunc = engine.GetValue("process")

        // Executa a função process
        let result = 
            try processFunc.Call(JsValue.Null, [| jsInput |])
            with ex -> raise (ArgumentException $"Error calling process: {ex.Message}")

        let resultObj = result.ToObject()

        let expectedObj = JsonSerializer.Deserialize<obj>(expectedOutput)
        let expectedJson = JsonSerializer.Serialize(expectedObj, JsonSerializerOptions(WriteIndented = false))
        let resultJson   = JsonSerializer.Serialize(resultObj)

        if expectedJson.Equals(resultJson) then
            true
        else
            raise (ArgumentException $"Resultado incorreto. Esperado: {expectedJson}, Obtido: {resultJson}")
    
    let validateScript (content: string) (inputJson: string) (expectedOutput: string): bool =
        validateContent content |> ignore
        let result = executeWithTestData content inputJson expectedOutput
        result
