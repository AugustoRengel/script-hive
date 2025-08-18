using ScriptHive.Application.DTOs.UserDTOs;

namespace ScriptHive.Application.Interfaces.UserInterfaces;

public interface IUserService
{
    Task CreateAsync(UserRequestDTO dto);
    Task<IEnumerable<UserResponseDTO>> GetAllAsync();
    Task<UserResponseDTO?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UserRequestDTO dto);
    Task DeleteAsync(Guid id);
}
