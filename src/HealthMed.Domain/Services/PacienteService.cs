using HealthMed.Domain.Entities;
using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Interfaces.Services;

namespace HealthMed.Domain.Services;

public class PacienteService(IPacienteRepository pacienteRepository) : IPacienteService
{
    public async Task<Paciente> ObterPorId(Guid pacienteId)
    {
        return await pacienteRepository.ObterPorId(pacienteId) ??
               throw new PacienteNaoEncontradoException();
    }

    public async Task Cadastrar(Paciente paciente)
    {
        await pacienteRepository.Adicionar(paciente);
        var evento = new PacienteCadastradoEvent(paciente.Id, paciente.Email, paciente.Nome);
        paciente.AdicionarEvento(evento);
    }

    public async Task Atualizar(Guid pacienteId, string nome)
    {
        var paciente = await pacienteRepository.ObterPorId(pacienteId) ??
                       throw new PacienteNaoEncontradoException();
        paciente.Atualizar(nome);
        await pacienteRepository.Atualizar(paciente);
    }

    public async Task Excluir(Guid pacienteId)
    {
        var paciente = await pacienteRepository.ObterPorId(pacienteId) ??
                       throw new PacienteNaoEncontradoException();
        paciente.MarcarComoApagado();
        await pacienteRepository.Atualizar(paciente);
    }
}