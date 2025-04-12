using HealthMed.Application.Models.InputModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enums;
using Mapster;

namespace HealthMed.Application.Features.Usuarios.CadastrarUsuario;

public class CadastrarUsuarioProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CadastrarUsuarioProfile, Usuario>();
        config.NewConfig<CadastroMedicoInputModel, CadastrarUsuarioCommand>()
            .ConstructUsing(m => new CadastrarUsuarioCommand(m.Senha, TipoUsuario.Medico));
        
        config.NewConfig<CadastroPacienteInputModel, CadastrarUsuarioCommand>()
            .ConstructUsing(p => new CadastrarUsuarioCommand(p.Senha, TipoUsuario.Paciente));
    }
}