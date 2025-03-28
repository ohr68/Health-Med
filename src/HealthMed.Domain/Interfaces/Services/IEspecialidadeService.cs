using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Services;

public interface IEspecialidadeService
{
    Task<IEnumerable<Especialidade>?> ObterTodas(CancellationToken cancellationToken = default);
}