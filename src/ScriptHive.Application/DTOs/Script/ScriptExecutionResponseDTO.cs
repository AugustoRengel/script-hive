namespace ScriptHive.Application.DTOs.ScriptExecutionDTOs;

public record ScriptExecutionResponseDTO
{
    public Guid Id { get; init; }
    public object? Result { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}
