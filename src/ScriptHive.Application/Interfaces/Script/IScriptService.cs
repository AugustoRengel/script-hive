using ScriptHive.Application.Commands.ScriptCommands;
using ScriptHive.Application.DTOs.ScriptDTOs;
using ScriptHive.Application.DTOs.ScriptExecutionDTOs;

namespace ScriptHive.Application.Interfaces.ScriptInterfaces;

public interface IScriptService
{
    Task CreateAsync(CreateScriptCommand dto);
    Task<IEnumerable<ScriptResponseDTO>> GetAllAsync();
    Task<IEnumerable<ScriptExecutionResponseDTO>> GetAllExecutionsAsync();
    Task<ScriptResponseDTO?> GetByIdAsync(Guid id);
    Task<ScriptExecutionResponseDTO?> GetExecutionByIdAsync(Guid id);
    Task<string?> ExecuteByIdAsync(Guid id, string inputData);
    bool CancelExecution(Guid executionId);
    Task UpdateAsync(Guid id, CreateScriptCommand dto);
    Task DeleteAsync(Guid id);
}
