using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IMedicoRepository
{
    Task<Medico?> ObterPorId(Guid medicoId);
    Task<Medico?> ObterPorCrm(string crm);
    Task<IEnumerable<Medico>?> ObterTodos(Guid? especialidadeId = null);
    Task Adicionar(Medico medico);
    Task Atualizar(Medico medico);
    Task<List<DisponibilidadeMedico>?> ObterDisponibilidade(Guid medicoId);
}