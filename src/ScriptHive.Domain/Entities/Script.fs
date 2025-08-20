namespace ScriptHive.Domain.Entities.Script

open System
open ScriptHive.Domain.Helpers

[<CLIMutable>]
type Script =
    {
        Id: Guid
        Title: string
        Content: string
        OwnerId: Guid
        CreatedAt: DateTime
    }

type ScriptFactory =
    static member Create(title: string, content: string, inputTestData: string, outputTestData: string, ownerId: Guid) : Script =
        let scriptIsValid = ScriptHelper.validateScript content inputTestData outputTestData
        if not scriptIsValid then
            raise (InvalidOperationException "Script execution Failed")
        { 
            Id = Guid.NewGuid()
            Title = title
            Content = content
            OwnerId = ownerId
            CreatedAt = DateTime.UtcNow 
        }