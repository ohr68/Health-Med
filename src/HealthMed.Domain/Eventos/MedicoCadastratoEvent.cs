namespace HealthMed.Domain.Eventos;

public class MedicoCadastratoEvent
{
    public Guid MedicoId { get; private set; }
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public DateTime CriadoEm { get; }
    public string Tipo { get; }
    
    public MedicoCadastratoEvent(Guid medicoId, string email, string nome)
    {
        MedicoId = medicoId;
        Email = email;
        Nome = nome;
        CriadoEm = DateTime.UtcNow;
        Tipo = nameof(PacienteCadastradoEvent);
    }
}