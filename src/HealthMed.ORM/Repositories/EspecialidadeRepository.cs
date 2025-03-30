using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.ORM.Repositories;

public class EspecialidadeRepository(ApplicationDbContext context) : IEspecialidadeRepository
{
    public async Task<IEnumerable<Especialidade>?> ObterTodas(CancellationToken cancellationToken = default) =>
        await context.Especialidades
            .ToListAsync(cancellationToken);
}