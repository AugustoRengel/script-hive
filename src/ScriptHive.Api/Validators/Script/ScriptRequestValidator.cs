using FluentValidation;
using ScriptHive.Api.DTOs.ScriptDTOs;

namespace ScriptHive.Api.Validators.ScriptRequestValidator;

public class ScriptRequestValidator : AbstractValidator<ScriptRequestDTO>
{
    public ScriptRequestValidator()
    {
        // Title - obrigatório e com tamanho mínimo/máximo
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(100).WithMessage("O título não pode ter mais que 100 caracteres.");

        // ContentFile - obrigatório, tamanho máximo e extensão permitida
        RuleFor(x => x.ContentFile)
            .NotNull().WithMessage("O arquivo de script é obrigatório.")
            .Must(file => file.Length > 0).WithMessage("O arquivo não pode estar vazio.")
            .Must(file => file.Length <= 1024 * 1024) // max de 1MB
                .WithMessage("O arquivo deve ter no máximo 1MB.")
            .Must(file => file.FileName.EndsWith(".js"))
                .WithMessage("O arquivo deve ser um script válido (.js).");

        // InputTestData - obrigatório, tamanho máximo e extensão permitida
        RuleFor(x => x.InputTestData)
            .NotNull().WithMessage("O arquivo json com dados de entrada para teste do script é obrigatório.")
            .Must(file => file.Length > 0).WithMessage("O arquivo json com dados de entrada não pode estar vazio.")
            .Must(file => file.Length <= 1024 * 1024) // max de 1MB
                .WithMessage("O arquivo json com dados de entrada deve ter no máximo 1MB.")
            .Must(file => file.FileName.EndsWith(".json"))
                .WithMessage("O arquivo json com dados de entrada deve ser um .json");

        // OutputTestData - obrigatório, tamanho máximo e extensão permitida
        RuleFor(x => x.OutputTestData)
            .NotNull().WithMessage("O arquivo json com dados de saida para teste do script é obrigatório.")
            .Must(file => file.Length > 0).WithMessage("O arquivo json com dados de saida não pode estar vazio.")
            .Must(file => file.Length <= 1024 * 1024) // max de 1MB
                .WithMessage("O arquivo json com dados de saida deve ter no máximo 1MB.")
            .Must(file => file.FileName.EndsWith(".json"))
                .WithMessage("O arquivo json com dados de saida deve ser um .json");
    }
}
