using FluentValidation;
using ScriptHive.Application.Commands.ScriptCommands;

namespace ScriptHive.Application.Validators.ScriptValidator;

public class ScriptValidator : AbstractValidator<CreateScriptCommand>
{
    public ScriptValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título do script é obrigatório.")
            .MaximumLength(100).WithMessage("O título do script deve ter no máximo 100 caracteres.");
        RuleFor(x => x.Content)
            .NotNull().WithMessage("O conteúdo do script é obrigatório.")
            .Must(c => c.Length > 0).WithMessage("O conteúdo do script não pode estar vazio.")
            .Must(c => c.Length <= 5000).WithMessage("O conteúdo do script deve ter no máximo 5000 bytes.");
        RuleFor(x => x.InputTestData)
            .NotEmpty().WithMessage("Os dados de entrada para testar o script são obrigatórios.");
        RuleFor(x => x.OutputTestData)
            .NotEmpty().WithMessage("Os dados de saida para testar o script são obrigatórios.");
    }
}
