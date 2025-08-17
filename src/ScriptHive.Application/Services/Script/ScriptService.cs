using ScriptHive.Application.DTOs.ScriptDTOs;
using ScriptHive.Application.Interfaces.ScriptInterfaces;
using ScriptHive.Application.Mapping.ScriptMapping;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;

using FluentValidation;

namespace ScriptHive.Application.Services.ScriptServices;

public class ScriptService(IScriptRepository repository, IValidator<ScriptRequestDTO> validator) : IScriptService
{
    private readonly IScriptRepository _repository = repository;
    private readonly IValidator<ScriptRequestDTO> _validator = validator;

    public async Task CreateAsync(ScriptRequestDTO dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        var entity = dto.ToEntity();

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

    public async Task UpdateAsync(Guid id, ScriptRequestDTO dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        var existing = await _repository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException("Script não encontrado.");

        var updated = existing.ToUpdatedEntity(dto);

        await _repository.UpdateAsync(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException("Script não encontrado.");

        await _repository.DeleteAsync(id);
    }
}
