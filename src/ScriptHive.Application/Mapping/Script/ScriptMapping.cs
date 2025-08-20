using ScriptHive.Application.Commands.ScriptCommands;
using ScriptHive.Application.DTOs.ScriptDTOs;
using ScriptHive.Domain.Entities.Script;

namespace ScriptHive.Application.Mapping.ScriptMapping;

public static class ScriptMapping
{
    public static Script ToEntity(this CreateScriptCommand command)
        => ScriptFactory.Create(
            title: command.Title, 
            content: command.Content,
            ownerId: command.OwnerId
        );

    public static Script ToUpdatedEntity(this Script existing, CreateScriptCommand command)
        => new Script(
            id: existing.Id,
            title: command.Title,
            content: command.Content,
            ownerId: existing.OwnerId,
            createdAt: existing.CreatedAt
        );

    public static ScriptResponseDTO ToResponse(this Script entity)
        => new ScriptResponseDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            OwnerId = entity.OwnerId,
            CreatedAt = entity.CreatedAt
        };
}
