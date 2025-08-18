using ScriptHive.Domain.ValueObjects.UserRole;

namespace ScriptHive.Application.DTOs.UserDTOs;

public record UserResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}