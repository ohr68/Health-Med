﻿using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Interfaces.Services;

public interface IMedicoService
{
    Task<Medico> ObterPorId(Guid medicoId, CancellationToken cancellationToken = default);
    Task<Medico?> ObterPorCrm(string crm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Medico>?> ObterTodos(CancellationToken cancellationToken = default);
    Task Cadastrar(Medico medico, string senha, CancellationToken cancellationToken = default);
    Task Atualizar(Guid medicoId, string nome, decimal valorConsulta);
    Task Excluir(Guid medicoId);
    Task AtualizarDisponibilidade(Guid medicoId, IEnumerable<DisponibilidadeMedico> disponibilidade);
}