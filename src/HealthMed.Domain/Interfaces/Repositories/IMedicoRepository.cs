using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IMedicoRepository
{
    Task<Medico?> ObterPorId(Guid medicoId, CancellationToken cancellationToken = default);
    Task<Medico?> ObterPorCrm(string crm, CancellationToken cancellationToken = default);
    Task<Medico?> ObterPorEmail(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Medico>?> ObterTodos(Guid? especialidadeId = null, CancellationToken cancellationToken = default);
    Task Adicionar(Medico medico);
    void Atualizar(Medico medico);
    Task<List<DisponibilidadeMedico>?> ObterDisponibilidade(Guid medicoId, CancellationToken cancellationToken = default);
    Task AtualizarDisponibilidade(Guid medicoId, IEnumerable<DisponibilidadeMedico> disponibilidade);
}