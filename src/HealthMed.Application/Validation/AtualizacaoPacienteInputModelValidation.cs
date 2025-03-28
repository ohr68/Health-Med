using FluentValidation;
using HealthMed.Application.Models.InputModels;

namespace HealthMed.Application.Validation;

public class AtualizacaoPacienteInputModelValidation : AbstractValidator<AtualizacaoPacienteInputModel>
{
    public AtualizacaoPacienteInputModelValidation()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.")
            .MaximumLength(200)
            .WithMessage("{PropertyName} deve ter menos do que {MaxLength} caracteres.");
    }
}