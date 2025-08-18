namespace ScriptHive.Domain.Interfaces.AuthInterfaces

open ScriptHive.Domain.Entities.User
open System.Threading.Tasks

type IAuthRepository =
    abstract member GetByUsernameAsync : string -> Task<User>

