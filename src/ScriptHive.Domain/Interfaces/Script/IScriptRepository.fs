namespace ScriptHive.Domain.Interfaces.ScriptInterfaces

open ScriptHive.Domain.Entities.Script
open System
open System.Threading.Tasks
open System.Collections.Generic

type IScriptRepository =
    abstract member GetAllAsync : unit -> Task<IEnumerable<Script>>
    abstract member GetByIdAsync : Guid -> Task<Script>
    abstract member CreateAsync : Script -> Task
    abstract member UpdateAsync : Script -> Task
    abstract member DeleteAsync : Guid -> Task

