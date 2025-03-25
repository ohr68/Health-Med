using HealthMed.Domain.Entities;
using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Exceptions.Medico;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Interfaces.Services;

namespace HealthMed.Domain.Services;

public class MedicoService(IMedicoRepository medicoRepository) : IMedicoService
{
    public async Task<Medico> ObterPorId(Guid medicoId) =>
        await medicoRepository.ObterPorId(medicoId) ??
        throw new MedicoNaoEncontradoException();

    public async Task<Medico> ObterPorCrm(string crm) =>
        await medicoRepository.ObterPorCrm(crm) ??
        throw new MedicoNaoEncontradoException();

    public async Task Cadastrar(Medico medico)
    {
        await medicoRepository.Adicionar(medico);
        
        var evento = new MedicoCadastradoEvent(medico.Id, medico.Email, medico.Nome);
        
        medico.AdicionarEvento(evento);
    }

    public async Task Atualizar(Guid medicoId, string nome, decimal valorConsulta)
    {
        var medico = await ObterPorId(medicoId);
        
        medico.Atualizar(nome, valorConsulta);
        
        await medicoRepository.Atualizar(medico);
    }

    public async Task Excluir(Guid medicoId)
    {
        var medico = await ObterPorId(medicoId);
        
        medico.MarcarComoApagado();
        
        await medicoRepository.Atualizar(medico);
    }
}