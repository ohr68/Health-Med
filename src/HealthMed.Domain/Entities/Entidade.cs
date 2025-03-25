using HealthMed.Domain.Events;

namespace HealthMed.Domain.Entities;

public abstract class Entidade
{
    public Guid Id { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }
    public bool Apagado { get; private set; }
    
    private List<IEventoDominio> _eventos = new List<IEventoDominio>();
    
    public IReadOnlyCollection<IEventoDominio> Eventos => _eventos.AsReadOnly();

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
        
    public void AdicionarEvento(IEventoDominio eventoDominio)
    {
        _eventos.Add(eventoDominio);
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