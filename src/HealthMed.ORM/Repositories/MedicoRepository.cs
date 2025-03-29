using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.ORM.Repositories;

internal class MedicoRepository(ApplicationDbContext context) : IMedicoRepository
{
    public async Task<Medico?> ObterPorId(Guid medicoId, CancellationToken cancellationToken = default) =>
        await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == medicoId, cancellationToken);

    public async Task<Medico?> ObterPorCrm(string crm, CancellationToken cancellationToken = default) =>
        await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Crm.ToLower() == crm.ToLower(), cancellationToken);

    public async Task<Medico?> ObterPorEmail(string email, CancellationToken cancellationToken = default) =>
        await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Valor == email, cancellationToken);

    public async Task<IEnumerable<Medico>?> ObterTodos(Guid? especialidadeId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Medicos.AsNoTracking();

        if (especialidadeId.HasValue)
            query = query.Where(x => x.EspecialidadeId == especialidadeId);

        return await query
            .OrderBy(x => x.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task Adicionar(Medico medico) => await context.AddAsync(medico);

    public void Atualizar(Medico medico) => context.Update(medico);

    public async Task AtualizarDisponibilidade(Guid medicoId, IEnumerable<DisponibilidadeMedico> disponibilidade)
    {
        var disponibilidadesRemover = await context.DisponibilidadeMedicos
            .Where(x => x.MedicoId == medicoId)
            .ToListAsync();

        if (disponibilidadesRemover.Any())
            foreach (var d in disponibilidadesRemover)
                context.DisponibilidadeMedicos.Remove(d);
        
        context.DisponibilidadeMedicos.AddRange(disponibilidade);
    }

    public async Task<List<DisponibilidadeMedico>?> ObterDisponibilidade(Guid medicoId,
        CancellationToken cancellationToken = default) =>
        await context.DisponibilidadeMedicos
            .AsNoTracking()
            .Where(x => x.MedicoId == medicoId)
            .ToListAsync(cancellationToken);
}