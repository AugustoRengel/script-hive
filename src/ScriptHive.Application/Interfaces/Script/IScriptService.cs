using ScriptHive.Application.Commands.ScriptCommands;
using ScriptHive.Application.DTOs.ScriptDTOs;

namespace ScriptHive.Application.Interfaces.ScriptInterfaces;

public interface IScriptService
{
    Task CreateAsync(CreateScriptCommand dto);
    Task<IEnumerable<ScriptResponseDTO>> GetAllAsync();
    Task<ScriptResponseDTO?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, CreateScriptCommand dto);
    Task DeleteAsync(Guid id);
}
