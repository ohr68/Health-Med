using FluentValidation;
using HealthMed.Application.Models.InputModels;

namespace HealthMed.Application.Validation;

public class CadastroMedicoInputModelValidation : AbstractValidator<CadastroMedicoInputModel>
{
    public CadastroMedicoInputModelValidation()
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
            
        RuleFor(p => p.Crm)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.")
            .MaximumLength(100)
            .WithMessage("{PropertyName} deve ter menos do que {MaxLength} caracteres.");

        RuleFor(p => p.ValorConsulta)
            .GreaterThan(0)
            .WithMessage("{PropertyName} é obrigatório.");
        
        RuleFor(p => p.EspecialidadeId)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.");
    }
}