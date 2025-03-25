using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IConsultaRepository
{
    Task<Consulta> ObterPorId(Guid consultaId);
    Task<IEnumerable<Consulta>?> ObterConsultasPaciente(Guid pacienteId);
    Task<IEnumerable<Consulta>?> ObterConsultasMedico(Guid medicoId);
    Task<IEnumerable<Consulta>?> ObterConsultasPendentesMedico(Guid medicoId);
    Task Adicionar(Consulta consulta);
    Task Atualizar(Consulta consulta);
}