using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IConsultaRepository
{
    Task<Consulta?> ObterPorId(Guid consultaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Consulta>?> ObterConsultasPaciente(Guid pacienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Consulta>?> ObterConsultasMedico(Guid medicoId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Consulta>?> ObterConsultasPendentesMedico(Guid medicoId,
        CancellationToken cancellationToken = default);

    Task Adicionar(Consulta consulta);
    void Atualizar(Consulta consulta);
}