using HealthMed.Domain.Events;

namespace HealthMed.Domain.Entities;

public abstract class Entidade
{
    public Guid Id { get; set; }
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; private set; }
    public bool Apagado { get; private set; } = false;

    private readonly List<IEventoDominio> _eventos = [];

    public IReadOnlyCollection<IEventoDominio> Eventos => _eventos.AsReadOnly();

    public void MarcarComoApagado()
    {
        Apagado = true;
        DefinirAtualizacao();
    }

    protected void DefinirAtualizacao() => AtualizadoEm = DateTime.UtcNow;

    public void AdicionarEvento(IEventoDominio eventoDominio) => _eventos.Add(eventoDominio);

    public bool PossuiEventos() => _eventos?.Any() ?? false;

    public override bool Equals(object? obj)
    {
        if (obj is not Entidade x)
            return false;

        return Id == x.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}