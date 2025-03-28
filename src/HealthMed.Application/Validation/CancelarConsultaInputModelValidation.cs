using FluentValidation;
using HealthMed.Application.Models.InputModels;

namespace HealthMed.Application.Validation;

public class CancelarConsultaInputModelValidation : AbstractValidator<CancelarConsultaInputModel>
{
    public CancelarConsultaInputModelValidation()
    {
        RuleFor(x => x.JustificativaCancelamento)
            .NotEmpty()
            .WithMessage("Informe a justificativa do cancelamento.");
    }
}