namespace ScriptHive.Domain.Interfaces.ScriptInterfaces

open ScriptHive.Domain.Entities.ScriptExecution
open System
open System.Threading.Tasks
open System.Collections.Generic

type IScriptExecutionRepository =
    abstract member GetAllAsync : unit -> Task<IEnumerable<ScriptExecution>>
    abstract member GetByIdAsync : Guid -> Task<ScriptExecution>
    abstract member CreateAsync : ScriptExecution -> Task
    abstract member UpdateAsync : ScriptExecution -> Task
    abstract member DeleteAsync : Guid -> Task

