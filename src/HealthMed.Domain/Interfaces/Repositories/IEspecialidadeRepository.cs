using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IEspecialidadeRepository
{
    Task<IEnumerable<Especialidade>?> ObterTodas(CancellationToken cancellationToken = default);
}