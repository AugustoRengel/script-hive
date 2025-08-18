using ScriptHive.Domain.ValueObjects.UserRole;

namespace ScriptHive.Application.DTOs.UserDTOs;

public record UserRequestDTO
(
    string Name,
    string Email,
    string Role,
    string Password
);