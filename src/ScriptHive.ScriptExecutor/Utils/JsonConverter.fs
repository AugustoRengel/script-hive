namespace ScriptHive.ScriptExecutor.Utils

open System
open System.Text.Json

open Jint
open Jint.Native

module public JsonConverter =

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