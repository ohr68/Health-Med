using FluentValidation;
using HealthMed.Application.Models.InputModels;

namespace HealthMed.Application.Validation;

public class CadastroPacienteInputModelValidation : AbstractValidator<CadastroPacienteInputModel>
{
    public CadastroPacienteInputModelValidation()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.")
            .MaximumLength(200)
            .WithMessage("{PropertyName} deve ter menos do que {MaxLength} caracteres.");
        
        RuleFor(p => p.Email)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.")
            .EmailAddress()
            .WithMessage("{PropertyName} inválido.");
    }
}