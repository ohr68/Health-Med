using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IPacienteRepository
{
    Task<Paciente?> ObterPorId(Guid pacienteId, CancellationToken cancellationToken = default);
    Task<Paciente?> ObterPorEmail(string email, CancellationToken cancellationToken = default);
    Task Adicionar(Paciente paciente, CancellationToken cancellationToken = default);
    void Atualizar(Paciente paciente);
}