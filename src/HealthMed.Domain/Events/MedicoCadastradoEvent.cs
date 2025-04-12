namespace HealthMed.Domain.Events;

public class MedicoCadastradoEvent(Guid medicoId, string email, string nome, string crm, string senha)
    : IEventoDominio
{
    public Guid MedicoId { get; private set; } = medicoId;
    public string Email { get; private set; } = email;
    public string Nome { get; private set; } = nome;
    public string Crm { get; private set; } = crm;
    public string Senha { get; private set; } = senha;
    public DateTime CriadoEm { get; } = DateTime.UtcNow;
    public string Tipo { get; } = nameof(MedicoCadastradoEvent);
}