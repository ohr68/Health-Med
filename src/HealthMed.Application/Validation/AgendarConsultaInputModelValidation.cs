using FluentValidation;
using HealthMed.Application.Models.InputModels;

namespace HealthMed.Application.Validation;

public class AgendarConsultaInputModelValidation : AbstractValidator<AgendarConsultaInputModel>
{
    public AgendarConsultaInputModelValidation()
    {
        RuleFor(x => x.MedicoId)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.");
        
        RuleFor(x => x.PacienteId)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.");
        
        RuleFor(x => x.Horario)
            .NotEmpty()
            .WithMessage("{PropertyName} é obrigatório.");
    }
}