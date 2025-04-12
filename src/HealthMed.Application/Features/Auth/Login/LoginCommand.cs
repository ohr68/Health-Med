using HealthMed.Application.Models.Requests;
using HealthMed.Domain.Enums;
using MediatR;

namespace HealthMed.Application.Features.Auth.Login;

public class LoginCommand : AuthRequestModel, IRequest<LoginResult>
{
    public string? Usuario { get; set; }
    public string? Senha { get; set; }
    public TipoUsuario? TipoUsuario { get; set; }
}