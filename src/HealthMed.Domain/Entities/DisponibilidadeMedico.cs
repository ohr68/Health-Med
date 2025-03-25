using HealthMed.Domain.Exceptions;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Entities;

public class DisponibilidadeMedico(int diaSemana, int horaInicio, int horaFim) : Entidade
{
    public DisponibilidadeMedico(Guid medicoId, int diaSemana, int horaInicio, int horaFim) : this(diaSemana,
        horaInicio, horaFim)
    {
        if (medicoId == Guid.Empty)
            throw new DomainException("O médico deve ser informado.");

        MedicoId = medicoId;
    }

    public Guid MedicoId { get; private set; }
    public DiaSemana DiaSemana { get; private set; } = diaSemana;
    public Hora HoraInicio { get; private set; } = horaInicio;
    public Hora HoraFim { get; private set; } = horaFim;
}