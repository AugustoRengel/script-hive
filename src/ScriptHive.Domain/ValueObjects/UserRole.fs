namespace ScriptHive.Domain.ValueObjects.UserRole

open System

type UserRole =
    | Admin
    | User
    | Guest
    override this.ToString() =
        match this with
        | Admin -> "Admin"
        | User -> "User"
        | Guest -> "Guest"

module UserRoleHelper =
    let private stringToUserRole =
        dict [
            "admin", Admin
            "user", User
            "guest", Guest
        ]

    [<CompiledName("ParseUserRole")>]
    let parseUserRole (roleStr: string) =
        match stringToUserRole.TryGetValue(roleStr.Trim().ToLower()) with
        | true, role -> role
        | false, _ -> raise (ArgumentException $"Invalid user role: {roleStr}")