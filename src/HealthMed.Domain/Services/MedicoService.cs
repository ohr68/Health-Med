using HealthMed.Domain.Entities;
using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Exceptions.Medico;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Interfaces.Services;

namespace HealthMed.Domain.Services;

public class MedicoService(IMedicoRepository medicoRepository) : IMedicoService
{
    public async Task<Medico> ObterPorId(Guid medicoId, CancellationToken cancellationToken = default) =>
        await medicoRepository.ObterPorId(medicoId, cancellationToken) ??
        throw new MedicoNaoEncontradoException();

    public async Task<Medico?> ObterPorCrm(string crm, CancellationToken cancellationToken = default) =>
        await medicoRepository.ObterPorCrm(crm, cancellationToken);

    public async Task<IEnumerable<Medico>?> ObterTodos(CancellationToken cancellationToken = default)
        => await medicoRepository.ObterTodos(null, cancellationToken);

    public async Task Cadastrar(Medico medico)
    {
        await VerificarEmailEmUso(medico);

        await VerificarCrmEmUso(medico);

        await medicoRepository.Adicionar(medico);

        var evento = new MedicoCadastradoEvent(medico.Id, medico.Email, medico.Nome);

        medico.AdicionarEvento(evento);
    }

    public async Task Atualizar(Guid medicoId, string nome, decimal valorConsulta)
    {
        var medico = await ObterPorId(medicoId);

        medico.Atualizar(nome, valorConsulta);

        medicoRepository.Atualizar(medico);
    }

    public async Task Excluir(Guid medicoId)
    {
        var medico = await ObterPorId(medicoId);

        medico.MarcarComoApagado();

        medicoRepository.Atualizar(medico);
    }

    public async Task AtualizarDisponibilidade(Guid medicoId, IEnumerable<DisponibilidadeMedico> disponibilidade)
    {
        var medico = await ObterPorId(medicoId);
        
        medico.AtualizarDisponibilidade(disponibilidade);
        
        await medicoRepository.AtualizarDisponibilidade(medicoId, medico.Disponibilidade!);
    }

    private async Task VerificarEmailEmUso(Medico medico)
    {
        var medicoBd = await medicoRepository.ObterPorEmail(medico.Email);

        if (medicoBd is not null)
            throw new EmailJaEstaEmUsoException();
    }

    private async Task VerificarCrmEmUso(Medico medico)
    {
        var medicoBd = await medicoRepository.ObterPorCrm(medico.Crm);

        if (medicoBd is not null)
            throw new CrmJaEstaEmUsoException();
    }
}