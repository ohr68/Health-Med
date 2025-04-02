using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;
using Mapster;

namespace HealthMed.Application.Mapster;

public class EspecialidadeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Especialidade, EspecialidadeViewModel>()
            .ConstructUsing(src => new EspecialidadeViewModel(src.Id, src.Nome));
    }
}