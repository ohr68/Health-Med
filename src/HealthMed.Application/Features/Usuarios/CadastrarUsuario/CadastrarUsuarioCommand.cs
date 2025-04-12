using HealthMed.Domain.Enums;
using MediatR;

namespace HealthMed.Application.Features.Usuarios.CadastrarUsuario;

public class CadastrarUsuarioCommand : IRequest<CadastrarUsuarioResult>
{
    public string? Senha { get; set; }
    public TipoUsuario TipoUsuario { get; set; }

    public CadastrarUsuarioCommand()
    {
        
    }

    public CadastrarUsuarioCommand(string? senha, TipoUsuario tipoUsuario)
    {
        Senha = senha;
        TipoUsuario = tipoUsuario;
    }
}