using ScriptHive.Application.DTOs.UserDTOs;
using ScriptHive.Domain.Entities.User;

namespace ScriptHive.Application.Mapping.UserMapping;

public static class UserMapping
{
    public static User ToEntity(this UserRequestDTO dto)
        => UserFactory.Create(
            name: dto.Name, 
            email: dto.Email, 
            role: dto.Role, 
            password: dto.Password
        );

    public static User ToUpdatedEntity(this User existing, UserRequestDTO dto)
        => UserFactory.Update(
            user: existing,
            name: dto.Name,
            email: dto.Email,
            role: dto.Role,
            password: dto.Password
        );

    public static UserResponseDTO ToResponse(this User entity)
        => new UserResponseDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Role = entity.Role.ToString(),
            CreatedAt = entity.CreatedAt
        };
}

