namespace HealthMed.Domain.Eventos;

public interface IEventoDominio
{
    DateTime CriadoEm { get; }
    string Tipo { get; }
}