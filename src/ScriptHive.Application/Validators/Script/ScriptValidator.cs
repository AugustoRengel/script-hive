using ScriptHive.Application.DTOs.ScriptDTOs;
using FluentValidation;

namespace ScriptHive.Application.Validators.ScriptValidator;

public class ScriptValidator : AbstractValidator<ScriptRequestDTO>
{
    public ScriptValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título do script é obrigatório.")
            .MaximumLength(100).WithMessage("O título do script deve ter no máximo 100 caracteres.");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("O conteúdo do script é obrigatório.")
            .MaximumLength(5000).WithMessage("O conteúdo do script deve ter no máximo 5000 caracteres.");
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("O ID do proprietário do script é obrigatório.");
    }
}
