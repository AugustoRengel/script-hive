using ScriptHive.Application.DTOs.AuthDTOs;
using FluentValidation;

namespace ScriptHive.Application.Validators.AuthValidator;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Usuário é obrigatório.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.");
    }
}
