namespace HealthMed.Domain.Events;

public class MedicoCadastradoEvent : IEventoDominio
{
    public Guid MedicoId { get; private set; }
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public DateTime CriadoEm { get; }
    public string Tipo { get; }
    
    public MedicoCadastradoEvent(Guid medicoId, string email, string nome)
    {
        MedicoId = medicoId;
        Email = email;
        Nome = nome;
        CriadoEm = DateTime.UtcNow;
        Tipo = nameof(PacienteCadastradoEvent);
    }
}