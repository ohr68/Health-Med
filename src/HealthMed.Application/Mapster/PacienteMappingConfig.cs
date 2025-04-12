using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;
using Mapster;

namespace HealthMed.Application.Mapster;

public class PacienteMappingConfig : IRegister 
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CadastroPacienteInputModel, Paciente>()
            .ConstructUsing(src => new Paciente(src.Nome, src.Email, src.Cpf));

        config.NewConfig<Paciente, PacienteViewModel>()
            .ConstructUsing(src => new PacienteViewModel(src.Id, src.Nome, src.Email));
    }
}