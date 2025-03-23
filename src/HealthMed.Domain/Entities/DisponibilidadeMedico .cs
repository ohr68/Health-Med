using HealthMed.Domain.Exceptions;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Entities;

public class DisponibilidadeMedico : Entidade
{
    public DisponibilidadeMedico(Guid medicoId, int diaSemana, int horaInicio, int horaFim)
    {
        if (medicoId == Guid.Empty)
            throw new DomainException("O médico deve ser informado.");

        MedicoId = medicoId;
        DiaSemana = diaSemana;
        HoraInicio = horaInicio;
        HoraFim = horaFim;
    }

    public Guid MedicoId { get; private set; }
    public DiaSemana DiaSemana { get; private set; }
    public Hora HoraInicio { get; private set; }
    public Hora HoraFim { get; private set; }
}