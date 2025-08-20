namespace ScriptHive.Domain.Interfaces.ScriptInterfaces

open ScriptHive.Domain.Entities.ScriptExecution
open System
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic

type IScriptExecutionResultQueue =
    abstract member EnqueueAsync: ScriptExecution -> Task

    abstract member DequeueAllAsync: cancellationToken: CancellationToken -> IAsyncEnumerable<ScriptExecution>