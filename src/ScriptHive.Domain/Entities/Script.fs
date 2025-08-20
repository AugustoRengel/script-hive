namespace ScriptHive.Domain.Entities.Script

open System

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
    static member Create(title: string, content: string, ownerId: Guid) : Script =
        { 
            Id = Guid.NewGuid()
            Title = title
            Content = content
            OwnerId = ownerId
            CreatedAt = DateTime.UtcNow 
        }