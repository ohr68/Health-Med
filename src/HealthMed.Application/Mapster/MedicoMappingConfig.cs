using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;
using Mapster;

namespace HealthMed.Application.Mapster;

public class MedicoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CadastroMedicoInputModel, Medico>()
            .ConstructUsing(src => new Medico(
                src.Nome,
                src.Email,
                src.Crm,
                src.EspecialidadeId,
                src.ValorConsulta,
                src.Disponibilidade.Adapt<List<DisponibilidadeMedico>>()));

        config.NewConfig<Medico, MedicoViewModel>()
            .ConstructUsing(src => new MedicoViewModel(
                src.Nome,
                src.Email,
                src.Crm,
                src.ValorConsulta,
                src.Especialidade.Adapt<EspecialidadeViewModel>(),
                src.Disponibilidade.Adapt<List<DisponibilidadeMedicoViewModel>>()));

        config.NewConfig<DisponibilidadeMedico, DisponibilidadeMedicoViewModel>()
            .ConstructUsing(src => new DisponibilidadeMedicoViewModel(
                src.DiaSemana, 
                src.DiaSemana.ToString(),
                src.HoraInicio, 
                src.HoraFim));

        config.NewConfig<DisponibilidadeMedicoInputModel, DisponibilidadeMedico>()
            .ConstructUsing(src => new DisponibilidadeMedico(
                src.DiaSemana, 
                src.HoraInicio, 
                src.HoraFim));
    }
}