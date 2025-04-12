using FluentValidation;

namespace HealthMed.Application.Features.Auth.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(l => l.UserId)
            .NotEmpty()
            .WithMessage("É obrigatório informar o id do usuário.");
    }
}