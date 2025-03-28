using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Services;

public interface IConsultaService
{
    Task<Consulta> ObterPorId(Guid consultaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Consulta>?> ObterConsultasPaciente(Guid pacienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Consulta>?> ObterConsultasMedico(Guid medicoId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Medico>?> ObterMedicos(Guid? especialidadeId = null,
        CancellationToken cancellationToken = default);

    Task Agendar(Consulta consulta);
    Task Cancelar(Guid consultaId, string justificativaCancelamento);
    Task Aceitar(Guid consultaId);
    Task Recusar(Guid consultaId);
}