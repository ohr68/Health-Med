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
    
    public virtual Paciente Paciente { get; protected set; }
    public virtual Medico Medico { get; protected set; }

    public bool HorarioUltrapassado => Horario > DateTime.UtcNow;

    public Consulta(Guid pacienteId, Guid medicoId, DateTime horario, decimal valor)
    {
        if (pacienteId == Guid.Empty)
            throw new DomainException("Informe o paciente da consulta.");
        
        if (medicoId == Guid.Empty)
            throw new DomainException("Informe o paciente da consulta.");
        
        if (horario.Equals(DateTime.MinValue))
            throw new DomainException("Informe o horário da consulta.");
        
        if (horario < DateTime.Now)
            throw new DomainException("Horário da consulta inválido.");
        
        if (valor <= 0)
            throw new DomainException("Valor da consulta inválido.");
        
        PacienteId = pacienteId;
        MedicoId = medicoId;
        Horario = horario;
        Valor = valor;
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
}