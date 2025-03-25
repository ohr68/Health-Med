using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Services;

public interface IConsultaService
{
    Task<Consulta> ObterPorId(Guid consultaId);
    Task<IEnumerable<Consulta>?> ObterConsultasPaciente(Guid pacienteId);
    Task<IEnumerable<Consulta>?> ObterConsultasMedico(Guid medicoId);
    Task<IEnumerable<Medico>?> ObterMedicos(Guid? especialidadeId = null);
    Task Agendar(Consulta consulta);
    Task Cancelar(Guid consultaId, string justificativaCancelamento);
    Task Aceitar(Guid consultaId);
    Task Recusar(Guid consultaId);
}