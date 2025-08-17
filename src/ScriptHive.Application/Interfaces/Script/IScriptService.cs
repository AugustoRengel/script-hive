using ScriptHive.Application.DTOs.ScriptDTOs;

namespace ScriptHive.Application.Interfaces.ScriptInterfaces;

public interface IScriptService
{
    Task CreateAsync(ScriptRequestDTO dto);
    Task<IEnumerable<ScriptResponseDTO>> GetAllAsync();
    Task<ScriptResponseDTO?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, ScriptRequestDTO dto);
    Task DeleteAsync(Guid id);
}
