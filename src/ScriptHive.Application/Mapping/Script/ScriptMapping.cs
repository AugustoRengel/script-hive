using ScriptHive.Application.DTOs.ScriptDTOs;
using ScriptHive.Domain.Entities.Script;

namespace ScriptHive.Application.Mapping.ScriptMapping;

public static class ScriptMappings
{
    public static Script ToEntity(this ScriptRequestDTO dto)
        => ScriptFactory.Create(dto.Title, dto.Content, dto.OwnerId);

    public static Script ToUpdatedEntity(this Script existing, ScriptRequestDTO dto)
        => new Script( // usa o construtor gerado do record F#
            id: existing.Id,
            title: dto.Title,
            content: dto.Content,
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
