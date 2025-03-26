﻿using HealthMed.Domain.Entities;
using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Exceptions.Paciente;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Interfaces.Services;

namespace HealthMed.Domain.Services;

public class PacienteService(IPacienteRepository pacienteRepository) : IPacienteService
{
    public async Task<Paciente> ObterPorId(Guid pacienteId) =>
        await pacienteRepository.ObterPorId(pacienteId) ??
        throw new PacienteNaoEncontradoException();

    public async Task Cadastrar(Paciente paciente)
    {
        await VerificarEmailEmUso(paciente);

        await pacienteRepository.Adicionar(paciente);

        var evento = new PacienteCadastradoEvent(paciente.Id, paciente.Email, paciente.Nome);

        paciente.AdicionarEvento(evento);
    }

    public async Task Atualizar(Guid pacienteId, string nome)
    {
        var paciente = await ObterPorId(pacienteId);

        paciente.Atualizar(nome);

        pacienteRepository.Atualizar(paciente);
    }

    public async Task Excluir(Guid pacienteId)
    {
        var paciente = await ObterPorId(pacienteId);

        paciente.MarcarComoApagado();

        pacienteRepository.Atualizar(paciente);
    }

    private async Task VerificarEmailEmUso(Paciente paciente)
    {
        var pacienteBd = await pacienteRepository.ObterPorEmail(paciente.Email);

        if (pacienteBd is not null)
            throw new EmailJaEstaEmUsoException();
    }
}