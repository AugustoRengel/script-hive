using ScriptHive.Application.DTOs.UserDTOs;
using ScriptHive.Application.Interfaces.UserInterfaces;
using ScriptHive.Application.Mapping.UserMapping;
using ScriptHive.Domain.Interfaces.UserInterfaces;

using FluentValidation;

namespace ScriptHive.Application.Services.UserServices;

public class UserService(IUserRepository repository, IValidator<UserRequestDTO> validator) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IValidator<UserRequestDTO> _validator = validator;

    public async Task CreateAsync(UserRequestDTO dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        var entity = dto.ToEntity();

        await _repository.CreateAsync(entity);
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(x => x.ToResponse());
    }

    public async Task<UserResponseDTO?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToResponse();
    }

    public async Task UpdateAsync(Guid id, UserRequestDTO dto)
    {
        await _validator.ValidateAndThrowAsync(dto);

        var existing = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User não encontrado.");

        var updated = existing.ToUpdatedEntity(dto);

        await _repository.UpdateAsync(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User não encontrado.");

        await _repository.DeleteAsync(id);
    }
}
