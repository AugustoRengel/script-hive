namespace ScriptHive.Domain.Helpers

open System
open System.Security.Cryptography

module PasswordHelper =
    let saltSize = 16
    let iterations = 100_000

    [<CompiledName("HashPassword")>]
    let hashPassword (password: string) =
        let salt = Array.zeroCreate<byte> saltSize
        use rng = RandomNumberGenerator.Create()
        rng.GetBytes(salt)
        use pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
        let hash = pbkdf2.GetBytes(32)
        $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}"

    [<CompiledName("VerifyPassword")>]
    let verifyPassword (password: string) (stored: string) =
        let parts = stored.Split('.')
        if parts.Length <> 2 then false
        else
            let salt = Convert.FromBase64String(parts.[0])
            let hashStored = Convert.FromBase64String(parts.[1])
            use pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
            let hashCheck = pbkdf2.GetBytes(32)
            hashCheck = hashStored

