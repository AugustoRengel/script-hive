using ScriptHive.Application.DTOs.UserDTOs;
using FluentValidation;

namespace ScriptHive.Application.Validators.UserValidator;

public class UserValidator : AbstractValidator<UserRequestDTO>
{
    public UserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("O role (Admin, User ou Guest) é obrigatório.");
    }
}
