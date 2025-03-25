using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Services;

public interface IMedicoService
{
    Task<Medico> ObterPorId(Guid medicoId);
    Task<Medico> ObterPorCrm(string crm);
    Task Cadastrar(Medico medico);
    Task Atualizar(Guid medicoId, string nome, decimal valorConsulta);
    Task Excluir(Guid medicoId);
}