namespace ScriptHive.Domain.ValueObjects.ExecutionStatus

open System

type ExecutionStatus =
    | Pending
    | Running
    | Completed
    | Failed

module ExecutionStatusHelper =
    let private stringToExecutionStatus =
        dict [
            "pending", Pending
            "running", Running
            "completed", Completed
            "failed", Failed
        ]

    [<CompiledName("ParseExecutionStatus")>]
    let parseExecutionStatus (executionStatus: string) =
        match stringToExecutionStatus.TryGetValue(executionStatus.Trim().ToLower()) with
        | true, status -> status
        | false, _ -> raise (ArgumentException $"Invalid execution status: {executionStatus}")