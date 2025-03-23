using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Entities;

public class Consulta : Entidade
{
    public Guid PacienteId { get; private set; }
    public Guid MedicoId { get; private set; }
    public DateTime Horario { get; private set; }
    public bool Cancelado { get; private set; }
    public virtual Paciente Paciente { get; protected set; }
    public virtual Medico Medico { get; protected set; }

    public Consulta(Guid pacienteId, Guid medicoId, DateTime horario)
    {
        if (pacienteId == Guid.Empty)
            throw new DomainException("Informe o paciente.");
        
        if (medicoId == Guid.Empty)
            throw new DomainException("Informe o paciente.");
        
        if (horario.Equals(DateTime.MinValue))
            throw new DomainException("Informe o horário.");
        
        if (horario < DateTime.Now)
            throw new DomainException("Horário inválido.");
        
        PacienteId = pacienteId;
        MedicoId = medicoId;
        Horario = horario;
        Cancelado = false;
    }

    public void Cancelar()
    {
        Cancelado = true;
        DefinirAtualizacao();
    }
}