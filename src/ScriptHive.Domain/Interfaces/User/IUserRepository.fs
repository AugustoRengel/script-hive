namespace ScriptHive.Domain.Interfaces.UserInterfaces

open ScriptHive.Domain.Entities.User
open System
open System.Threading.Tasks
open System.Collections.Generic

type IUserRepository =
    abstract member GetAllAsync : unit -> Task<IEnumerable<User>>
    abstract member GetByIdAsync : Guid -> Task<User>
    abstract member CreateAsync : User -> Task
    abstract member UpdateAsync : User -> Task
    abstract member DeleteAsync : Guid -> Task

