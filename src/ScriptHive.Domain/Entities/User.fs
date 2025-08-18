namespace ScriptHive.Domain.Entities.User

open System
open ScriptHive.Domain.ValueObjects.UserRole
open ScriptHive.Domain.Helpers

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

type UserFactory =
    static member Create(name: string, email: string, role: string, password: string) : User =
        let userRole = UserRoleHelper.parseUserRole role
        let passwordHash = PasswordHelper.hashPassword password
        { 
            Id = Guid.NewGuid()
            Name = name
            Email = email
            Role = userRole
            PasswordHash = passwordHash
            CreatedAt = DateTime.UtcNow 
        }

    static member Update(user: User, name: string, email: string, role: string, password: string) : User =
        let userRole = UserRoleHelper.parseUserRole role
        let passwordHash = PasswordHelper.hashPassword password
        { 
            Id = user.Id
            Name = name
            Email = email
            Role = userRole
            PasswordHash = passwordHash
            CreatedAt = user.CreatedAt
        }