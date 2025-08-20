namespace ScriptHive.ScriptExecutor.Execution

open System
open System.Text.Json

open Jint
open Jint.Native

open ScriptHive.ScriptExecutor.Validation
open ScriptHive.ScriptExecutor.Utils

module public ScriptRunner =

    let execute (processScript: string) (inputJson: string) : string =
        ScriptValidation.validateContent processScript

        let engine =
            new Engine(fun options ->
                options.LimitMemory(4_000_000) |> ignore
                options.TimeoutInterval(TimeSpan.FromSeconds(4.0)) |> ignore
                options.MaxStatements(1000) |> ignore
            )

        [ "console"; "require"; "process"; "fs"; "fetch"; "XMLHttpRequest" ]
        |> List.iter (fun name -> engine.SetValue(name, JsValue.Undefined) |> ignore)
        
        let doc = JsonDocument.Parse(inputJson)
        let jsInput = JsonConverter.jsonElementToJsValue engine doc.RootElement

        engine.Execute(processScript) |> ignore

        let processFunc = engine.GetValue("process")

        let result = processFunc.Call(JsValue.Null, [| jsInput |])
        let resultObj = result.ToObject()

        let resultJson   = JsonSerializer.Serialize(resultObj)
        resultJson

    let verifyScriptBehavior (processScript: string) (inputJson: string) (expectedOutput: string) =
        let resultJson = execute processScript inputJson
        let expectedObj = JsonSerializer.Deserialize<obj>(expectedOutput)
        let expectedJson = JsonSerializer.Serialize(expectedObj, JsonSerializerOptions(WriteIndented = false))
        if expectedJson <> resultJson then
            raise (ArgumentException $"Resultado incorreto. Esperado: {expectedJson}, Obtido: {resultJson}")