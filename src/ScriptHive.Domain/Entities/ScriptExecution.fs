namespace ScriptHive.Domain.Entities.ScriptExecution 

open System
open ScriptHive.Domain.ValueObjects.ExecutionStatus

[<CLIMutable>]
type ScriptExecution =
    { 
        Id: Guid
        ScriptId: Guid
        Input: string
        Status: ExecutionStatus
        Result: string
        CreatedAt: DateTime
        StartedAt: Nullable<DateTime>
        FinishedAt: Nullable<DateTime>
    }

type ScriptExecution with
    member this.MarkRunning() =
        { this with 
            Status = ExecutionStatus.Running 
            StartedAt = Nullable(DateTime.UtcNow) }

    member this.MarkCompleted(result: string) =
        { this with 
            Status = ExecutionStatus.Completed
            Result = result
            FinishedAt = Nullable(DateTime.UtcNow) }

    member this.MarkFailed(error: string) =
        { this with 
            Status = ExecutionStatus.Failed
            Result = error
            FinishedAt = Nullable(DateTime.UtcNow) }

type ScriptExecutionFactory =
    static member Create(scriptId: Guid, inputData: string) : ScriptExecution =
        { 
            Id = Guid.NewGuid()
            ScriptId = scriptId
            Input = inputData
            Status = ExecutionStatus.Pending
            Result = null
            CreatedAt = DateTime.UtcNow 
            StartedAt = Nullable() 
            FinishedAt = Nullable()
        }