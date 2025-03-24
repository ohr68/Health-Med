using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IPacienteRepository
{
    Task<Paciente?> ObterPorId(Guid pacienteId);
    Task Adicionar(Paciente paciente);
    Task Atualizar(Paciente paciente);
}