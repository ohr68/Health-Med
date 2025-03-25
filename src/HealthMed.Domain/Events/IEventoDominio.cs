namespace HealthMed.Domain.Events;

public interface IEventoDominio
{
    DateTime CriadoEm { get; }
    string Tipo { get; }
}