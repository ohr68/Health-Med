using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.ORM.Repositories;

internal class PacienteRepository(ApplicationDbContext context) : IPacienteRepository
{
    public async Task<Paciente?> ObterPorId(Guid pacienteId) =>
        await context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == pacienteId);

    public async Task<Paciente?> ObterPorEmail(string email) =>
        await context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email);

    public async Task Adicionar(Paciente paciente) => await context.AddAsync(paciente);

    public void Atualizar(Paciente paciente) => context.Update(paciente);
}