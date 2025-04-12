using FluentValidation;
using HealthMed.Application.CustomValidators;

namespace HealthMed.Application.Features.Usuarios.CadastrarUsuario;

public class CadastrarUsuarioCommandValidator : AbstractValidator<CadastrarUsuarioCommand>
{
    public CadastrarUsuarioCommandValidator()
    {
        RuleFor(c => c.Senha)
            .NotEmpty()
            .WithMessage("É obrigatório informar a senha.")
            .SetValidator(new StrongPasswordValidator<CadastrarUsuarioCommand>()!);
    }
}