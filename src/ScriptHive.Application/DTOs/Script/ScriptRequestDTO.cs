namespace ScriptHive.Application.DTOs.ScriptDTOs;

public record ScriptRequestDTO
(
    string Title,
    string Content,
    Guid OwnerId
);
