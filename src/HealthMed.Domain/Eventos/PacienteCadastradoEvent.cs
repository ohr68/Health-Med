namespace HealthMed.Domain.Eventos;

public class PacienteCadastradoEvent : IEventoDominio
{
    public Guid PacienteId { get; private set; }
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public DateTime CriadoEm { get; }
    public string Tipo { get; }
    
    public PacienteCadastradoEvent(Guid pacienteId, string email, string nome)
    {
        PacienteId = pacienteId;
        Email = email;
        Nome = nome;
        CriadoEm = DateTime.UtcNow;
        Tipo = nameof(PacienteCadastradoEvent);
    }
}