namespace ScriptHive.Domain.Entities.User

open System
open ScriptHive.Domain.ValueObjects.UserRole

[<CLIMutable>]
type User =
    {
        Id: Guid
        Name: string
        Email: string
        Role: UserRole
        PasswordHash: string
        CreatedAt: DateTime
    }