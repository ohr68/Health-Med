using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Services;

public interface IPacienteService
{
    Task<Paciente> ObterPorId(Guid pacienteId, CancellationToken cancellationToken = default);
    Task Cadastrar(Paciente paciente, string senha, CancellationToken cancellationToken = default);
    Task Atualizar(Guid pacienteId, string nome);
    Task Excluir(Guid pacienteId);
}