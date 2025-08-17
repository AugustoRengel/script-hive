namespace ScriptHive.Application.DTOs.ScriptDTOs;

public record ScriptResponseDTO
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public Guid OwnerId { get; init; }
    public DateTime CreatedAt { get; init; }
}
