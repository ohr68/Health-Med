using HealthMed.Domain.Exceptions;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Entities;

public class DisponibilidadeMedico : Entidade
{
    public Guid MedicoId { get; private set; }
    public DiaSemana DiaSemana { get; private set; }
    public Hora HoraInicio { get; private set; }
    public Hora HoraFim { get; private set; }
    public virtual Medico Medico { get; private set; }

    public DisponibilidadeMedico(int diaSemana, int horaInicio, int horaFim)
    {
        DiaSemana = diaSemana;
        HoraInicio = horaInicio;
        HoraFim = horaFim;
    }

    private DisponibilidadeMedico()
    {
    }
    
    public DisponibilidadeMedico(Guid medicoId, int diaSemana, int horaInicio, int horaFim) : this(diaSemana,
        horaInicio, horaFim)
    {
        if (medicoId == Guid.Empty)
            throw new DomainException("O médico deve ser informado.");

        MedicoId = medicoId;
    }
}