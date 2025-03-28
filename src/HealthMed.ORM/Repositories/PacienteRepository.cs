using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.ORM.Repositories;

internal class PacienteRepository(ApplicationDbContext context) : IPacienteRepository
{
    public async Task<Paciente?> ObterPorId(Guid pacienteId, CancellationToken cancellationToken = default) =>
        await context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == pacienteId, cancellationToken);

    public async Task<Paciente?> ObterPorEmail(string email, CancellationToken cancellationToken = default) =>
        await context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email.Valor == email, cancellationToken);

    public async Task Adicionar(Paciente paciente) => await context.Pacientes.AddAsync(paciente);

    public void Atualizar(Paciente paciente) => context.Pacientes.Update(paciente);
}