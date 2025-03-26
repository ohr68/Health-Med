using HealthMed.Domain.Entities;
using HealthMed.Domain.Enums;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.ORM.Repositories;

internal class ConsultaRepository(ApplicationDbContext context) : IConsultaRepository
{
    public async Task<Consulta?> ObterPorId(Guid consultaId) =>
        await context.Consultas
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == consultaId);

    public async Task<IEnumerable<Consulta>?> ObterConsultasPaciente(Guid pacienteId) =>
        await context
            .Consultas
            .AsNoTracking()
            .Where(c => c.PacienteId == pacienteId)
            .OrderBy(c => c.Horario)
            .ToListAsync();

    public async Task<IEnumerable<Consulta>?> ObterConsultasMedico(Guid medicoId) =>
        await context
            .Consultas
            .AsNoTracking()
            .Where(c => c.MedicoId == medicoId)
            .OrderBy(c => c.Horario)
            .ToListAsync();

    public async Task<IEnumerable<Consulta>?> ObterConsultasPendentesMedico(Guid medicoId) =>
        await context
            .Consultas
            .AsNoTracking()
            .Where(c => c.MedicoId == medicoId
                        && c.Horario >= DateTime.Now
                        && c.Status == StatusConsulta.Aceita)
            .ToListAsync();

    public async Task Adicionar(Consulta consulta) => await context.AddAsync(consulta);

    public void Atualizar(Consulta consulta) => context.Update(consulta);
}