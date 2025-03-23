using HealthMed.Domain.Eventos;

namespace HealthMed.Domain.Entities;

public abstract class Entidade
{
    public Guid Id { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }
    public bool Apagado { get; private set; }
    
    private List<IEventoDominio> _events = new List<IEventoDominio>();
    
    public IReadOnlyCollection<IEventoDominio> Events => _events.AsReadOnly();

    protected Entidade()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
        Apagado = false;
    }

    public void MarcarComoApagado()
    {
        Apagado = true;
        DefinirAtualizacao();
    }

    public void DefinirAtualizacao()
    {
        AtualizadoEm = DateTime.UtcNow;
    }
        
    public void AddEvento(IEventoDominio eventoDominio)
    {
        _events.Add(eventoDominio);
    }

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