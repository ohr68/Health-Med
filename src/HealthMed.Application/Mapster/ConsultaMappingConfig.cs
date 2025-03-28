using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;
using Mapster;

namespace HealthMed.Application.Mapster;

public class ConsultaMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AgendarConsultaInputModel, Consulta>()
            .ConstructUsing(src => new Consulta(
                src.PacienteId,
                src.MedicoId,
                src.Horario));

        config.NewConfig<Consulta, ConsultaViewModel>()
            .ConstructUsing(src => new ConsultaViewModel(
                src.Horario,
                src.ObterStatusDesc(),
                src.JustificativaCancelamento,
                src.Valor,
                src.Paciente.Adapt<PacienteViewModel>(),
                src.Medico.Adapt<MedicoViewModel>()));
    }
}