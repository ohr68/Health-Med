using FluentValidation;

namespace HealthMed.Application.Features.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(l => l.Usuario)
            .NotEmpty()
            .WithMessage("É obrigatório informar o nome do usuário.")
            .MaximumLength(100)
            .WithMessage("O nome de usuário deve possuir no máximo 100 caracteres.");

        RuleFor(l => l.Senha)
            .NotEmpty()
            .WithMessage("É obrigatório informar a senha.");
        
        RuleFor(l => l.TipoUsuario)
            .NotEmpty()
            .WithMessage("É obrigatório informar o tipo de usuário.")
            .IsInEnum()
            .WithMessage("Informe um tipo de usuário válido.");
    }
}