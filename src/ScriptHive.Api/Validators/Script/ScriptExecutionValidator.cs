using FluentValidation;
using ScriptHive.Api.DTOs.ScriptDTOs;

namespace ScriptHive.Api.Validators.ScriptExecutionValidator;

public class ScriptExecutionValidator : AbstractValidator<ScriptExecutionDTO>
{
    public ScriptExecutionValidator()
    {
        // InputTestData - obrigatório, tamanho máximo e extensão permitida
        RuleFor(x => x.InputData)
            .NotNull().WithMessage("O arquivo json com dados de entrada para execução do script é obrigatório.")
            .Must(file => file.Length > 0).WithMessage("O arquivo json com dados de entrada não pode estar vazio.")
            .Must(file => file.Length <= 1024 * 1024) // max de 1MB
                .WithMessage("O arquivo json com dados de entrada deve ter no máximo 1MB.")
            .Must(file => file.FileName.EndsWith(".json"))
                .WithMessage("O arquivo json com dados de entrada deve ser um .json");
    }
}
