using FluentValidation;
using HealthMed.Application.Models.InputModels;

namespace HealthMed.Application.Validation;

public class AtualizacaoMedicoInputModelValidation : AbstractValidator<AtualizacaoMedicoInputModel>
{
    public AtualizacaoMedicoInputModelValidation()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.")
            .MaximumLength(200)
            .WithMessage("{PropertyName} deve ter menos do que {MaxLength} caracteres.");
        
        RuleFor(p => p.ValorConsulta)
            .GreaterThan(0)
            .WithMessage("{PropertyName} é obrigatório.");
    }
}