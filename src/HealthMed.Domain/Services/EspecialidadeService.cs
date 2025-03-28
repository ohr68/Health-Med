using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Interfaces.Services;

namespace HealthMed.Domain.Services;

public class EspecialidadeService(IEspecialidadeRepository especialidadeRepository) : IEspecialidadeService
{
    public async Task<IEnumerable<Especialidade>?> ObterTodas(CancellationToken cancellationToken = default) =>
        await especialidadeRepository.ObterTodas(cancellationToken);
}