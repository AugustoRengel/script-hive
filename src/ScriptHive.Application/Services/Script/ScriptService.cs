using FluentValidation;
using ScriptHive.Application.Commands.ScriptCommands;
using ScriptHive.Application.DTOs.ScriptDTOs;
using ScriptHive.Application.Interfaces.ScriptInterfaces;
using ScriptHive.Application.Mapping.ScriptMapping;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.ScriptExecutor.Execution;

namespace ScriptHive.Application.Services.ScriptServices;

public class ScriptService(IScriptRepository repository, IValidator<CreateScriptCommand> validator) : IScriptService
{
    private readonly IScriptRepository _repository = repository;
    private readonly IValidator<CreateScriptCommand> _validator = validator;

    public async Task CreateAsync(CreateScriptCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);

        ScriptRunner.verifyScriptBehavior(
            processScript: command.Content, 
            inputJson: command.InputTestData, 
            expectedOutput: command.OutputTestData
        );

        var entity = command.ToEntity();

        await _repository.CreateAsync(entity);
    }

    public async Task<IEnumerable<ScriptResponseDTO>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(x => x.ToResponse());
    }

    public async Task<ScriptResponseDTO?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToResponse();
    }

    public async Task<string?> ExecuteByIdAsync(Guid id, string inputData)
    {
        var _script = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Script não encontrado.");

        var scriptOutput = ScriptRunner.execute(_script.Content, inputData);

        return scriptOutput;
    }

    public async Task UpdateAsync(Guid id, CreateScriptCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);

        var existing = await _repository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException("Script não encontrado.");

        ScriptRunner.verifyScriptBehavior(
            processScript: command.Content,
            inputJson: command.InputTestData,
            expectedOutput: command.OutputTestData
        );

        var updated = existing.ToUpdatedEntity(command);

        await _repository.UpdateAsync(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException("Script não encontrado.");

        await _repository.DeleteAsync(id);
    }
}
