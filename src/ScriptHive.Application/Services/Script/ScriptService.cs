using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ScriptHive.Application.Commands.ScriptCommands;
using ScriptHive.Application.DTOs.ScriptDTOs;
using ScriptHive.Application.DTOs.ScriptExecutionDTOs;
using ScriptHive.Application.Interfaces.ScriptInterfaces;
using ScriptHive.Application.Mapping.ScriptExecutionMapping;
using ScriptHive.Application.Mapping.ScriptMapping;
using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.ScriptExecutor.Execution;
using System;
using System.Collections.Concurrent;

namespace ScriptHive.Application.Services.ScriptServices;

public class ScriptService(
    IScriptRepository scriptRepository, 
    IScriptExecutionRepository scriptExecutionRepository, 
    IValidator<CreateScriptCommand> validator,
    IScriptExecutionResultQueue queue) : IScriptService
{
    private readonly IScriptRepository _repository = scriptRepository;
    private readonly IScriptExecutionRepository _executionRepository = scriptExecutionRepository;
    private readonly IValidator<CreateScriptCommand> _validator = validator;
    private readonly IScriptExecutionResultQueue _queue = queue;

    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _cancellationTokens = new();

    public async Task<Guid> CreateAsync(CreateScriptCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);

        var entity = command.ToEntity();

        var cts = new CancellationTokenSource();
        _cancellationTokens[entity.Id] = cts;

        ScriptRunner.verifyScriptBehavior(
            processScript: command.Content, 
            inputJson: command.InputTestData, 
            expectedOutput: command.OutputTestData,
            ct: cts.Token
        );

        await _repository.CreateAsync(entity);
        return entity.Id;
    }

    public async Task<IEnumerable<ScriptResponseDTO>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(x => x.ToResponse());
    }

    public async Task<IEnumerable<ScriptExecutionResponseDTO>> GetAllExecutionsAsync()
    {
        var list = await _executionRepository.GetAllAsync();
        return list.Select(x => x.ToResponse());
    }

    public async Task<ScriptResponseDTO?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToResponse();
    }

    public async Task<ScriptExecutionResponseDTO?> GetExecutionByIdAsync(Guid id)
    {
        var entity = await _executionRepository.GetByIdAsync(id);
        return entity?.ToResponse();
    }

    public async Task<string?> ExecuteByIdAsync(Guid id, string inputData)
    {
        var _script = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Script não encontrado.");

        var execution = ScriptExecutionFactory.Create(scriptId: _script.Id, inputData);
        execution = execution.MarkRunning();

        await _executionRepository.CreateAsync(execution);

        // Cria token para cancelamento
        var cts = new CancellationTokenSource();
        _cancellationTokens[execution.Id] = cts;

        // Dispara execução em background
        _ = Task.Run(async () =>
        {
            try
            {
                var result = ScriptRunner.execute(
                    _script.Content,
                    execution.Input,
                    cts.Token
                );

                execution = execution.MarkCompleted(result);
            }
            catch (Exception ex)
            {
                execution = execution.MarkFailed(ex.Message);
            }
            finally
            {
                await _queue.EnqueueAsync(execution);
                _cancellationTokens.TryRemove(execution.Id, out _);
            }
        });

        return execution.Id.ToString();
    }

    public bool CancelExecution(Guid executionId)
    {
        if (_cancellationTokens.TryGetValue(executionId, out var cts))
        {
            cts.Cancel();
            return true;
        }
        return false;
    }

    public async Task UpdateAsync(Guid id, CreateScriptCommand command)
    {
        await _validator.ValidateAndThrowAsync(command);

        var existing = await _repository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException("Script não encontrado.");

        // Cria token para cancelamento
        var cts = new CancellationTokenSource();
        _cancellationTokens[existing.Id] = cts;

        ScriptRunner.verifyScriptBehavior(
            processScript: command.Content,
            inputJson: command.InputTestData,
            expectedOutput: command.OutputTestData,
            ct: cts.Token
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
