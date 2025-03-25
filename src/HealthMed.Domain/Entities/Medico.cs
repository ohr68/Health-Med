using HealthMed.Domain.Exceptions;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Entities;

public class Medico : Entidade
{
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public string Crm { get; private set; }
    public Guid EspecialidadeId { get; private set; }
    public decimal ValorConsulta { get; private set; }

    public virtual Especialidade Especialidade { get; private set; }

    private readonly List<DisponibilidadeMedico>? _disponibilidade;

    public virtual IReadOnlyCollection<DisponibilidadeMedico>? Disponibilidade => _disponibilidade?.AsReadOnly();

    public Medico(string nome, string email, string crm, Guid especialidadeId, decimal valorConsulta,
        List<DisponibilidadeMedico>? disponibilidade)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("O nome do médico não pode ser vazio.");

        if (string.IsNullOrEmpty(email))
            throw new DomainException("O email do médico não pode ser vazio.");

        if (string.IsNullOrEmpty(crm))
            throw new DomainException("O CRM do médico não pode ser vazio.");

        if (especialidadeId == Guid.Empty)
            throw new DomainException("A especialidade do médico não pode ser vazia.");

        if (valorConsulta <= 0)
            throw new DomainException("O valor da consulta do médico não pode ser vazio.");

        Nome = nome;
        Email = email;
        Crm = crm;
        EspecialidadeId = especialidadeId;
        ValorConsulta = valorConsulta;

        if (disponibilidade is not null)
            _disponibilidade = disponibilidade;
    }

    public void Atualizar(string nome, decimal valorConsulta)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("O nome não pode ser vazio.");

        if (valorConsulta <= 0)
            throw new DomainException("O valor da consulta não pode ser vazio.");

        Nome = nome;
        ValorConsulta = valorConsulta;
        DefinirAtualizacao();
    }

    public bool PossuiDisponibilidade(int diaSemana, int hora)
    {
        if (Disponibilidade?.Any() ?? false)
            return Disponibilidade.Any(d =>
                d.DiaSemana == (int)diaSemana && hora >= d.HoraInicio && hora <= d.HoraFim);

        return true;
    }
}