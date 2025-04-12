using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.ORM.Repositories;

internal class PacienteRepository(HealthMedDbContext context) : IPacienteRepository
{
    public async Task<Paciente?> ObterPorId(Guid pacienteId, CancellationToken cancellationToken = default) =>
        await context.Pacientes
            .FirstOrDefaultAsync(p => p.Id == pacienteId, cancellationToken);

    public async Task<Paciente?> ObterPorEmail(string email, CancellationToken cancellationToken = default) =>
        await context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email.Valor == email, cancellationToken);

    public async Task Adicionar(Paciente paciente, CancellationToken cancellationToken = default) => await context.Pacientes.AddAsync(paciente, cancellationToken);

    public void Atualizar(Paciente paciente) => context.Pacientes.Update(paciente);
}