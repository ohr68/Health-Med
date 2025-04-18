using HealthMed.Domain.Enums;
using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Entities;

public class Consulta : Entidade
{
    public Guid PacienteId { get; private set; }
    public Guid MedicoId { get; private set; }
    public DateTime Horario { get; private set; }
    public StatusConsulta Status { get; private set; }
    public string? JustificativaCancelamento { get; private set; }
    public decimal Valor { get; private set; }

    public virtual Paciente? Paciente { get; protected set; }
    public virtual Medico? Medico { get; protected set; }

    public bool HorarioUltrapassado => DateTime.UtcNow > Horario.ToUniversalTime();

    private Consulta()
    {
    }

    public Consulta(Guid pacienteId, Guid medicoId, DateTime horario)
    {
        if (pacienteId == Guid.Empty)
            throw new DomainException("Informe o paciente da consulta.");

        if (medicoId == Guid.Empty)
            throw new DomainException("Informe o paciente da consulta.");

        if (horario.Equals(DateTime.MinValue))
            throw new DomainException("Informe o horário da consulta.");

        if (horario < DateTime.Now)
            throw new DomainException("Horário da consulta inválido.");

        PacienteId = pacienteId;
        MedicoId = medicoId;
        Horario = horario;
        Status = StatusConsulta.AguardandoAceite;
    }

    public void Cancelar(string justificativaCancelamento)
    {
        Status = StatusConsulta.Cancelada;

        if (string.IsNullOrWhiteSpace(justificativaCancelamento))
            throw new DomainException("Valor inválido.");

        JustificativaCancelamento = justificativaCancelamento;
        DefinirAtualizacao();
    }

    public void Aceitar()
    {
        Status = StatusConsulta.Aceita;
        DefinirAtualizacao();
    }

    public void Recusar()
    {
        Status = StatusConsulta.Recusada;
        DefinirAtualizacao();
    }

    public void DefinirValorConsulta(decimal valor)
    {
        if (valor <= 0)
            throw new DomainException("Valor da consulta inválido.");
        
        Valor = valor;
    }

    public string ObterStatusDesc() =>
        Status switch
        {
            StatusConsulta.AguardandoAceite => "Aguardando aceite",
            StatusConsulta.Aceita => "Aceita",
            StatusConsulta.Recusada => "Recusada",
            StatusConsulta.Cancelada => "Cancelada",
            _ => "Status desconhecido"
        };
}