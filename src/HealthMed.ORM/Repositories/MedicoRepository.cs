using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.ORM.Repositories;

internal class MedicoRepository(ApplicationDbContext context) : IMedicoRepository
{
    public async Task<Medico?> ObterPorId(Guid medicoId) =>
        await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == medicoId);

    public async Task<Medico?> ObterPorCrm(string crm) =>
        await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Crm.ToLower() == crm.ToLower());

    public async Task<Medico?> ObterPorEmail(string email) =>
        await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);

    public async Task<IEnumerable<Medico>?> ObterTodos(Guid? especialidadeId = null)
    {
        var query = context.Medicos.AsNoTracking();

        if (especialidadeId.HasValue)
            query = query.Where(x => x.EspecialidadeId == especialidadeId);

        return await query.ToListAsync();
    }

    public async Task Adicionar(Medico medico) => await context.AddAsync(medico);

    public void Atualizar(Medico medico) => context.Update(medico);

    public async Task<List<DisponibilidadeMedico>?> ObterDisponibilidade(Guid medicoId) =>
        await context.DisponibilidadeMedicos
            .AsNoTracking()
            .Where(x => x.MedicoId == medicoId)
            .ToListAsync();
}