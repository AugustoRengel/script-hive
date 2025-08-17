namespace ScriptHive.Domain.ValueObjects.UserRole

type UserRole =
    | Admin
    | User
    | Guest
    override this.ToString() =
        match this with
        | Admin -> "Admin"
        | User -> "User"
        | Guest -> "Guest"