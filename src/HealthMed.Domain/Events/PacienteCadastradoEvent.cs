namespace HealthMed.Domain.Events;

public class PacienteCadastradoEvent(Guid pacienteId, string email, string nome, string cpf, string senha)
    : IEventoDominio
{
    public Guid PacienteId { get; private set; } = pacienteId;
    public string Email { get; private set; } = email;
    public string Nome { get; private set; } = nome;
    public string Cpf { get; private set; } = cpf;
    public string Senha { get; private set; } = senha;
    public DateTime CriadoEm { get; } = DateTime.UtcNow;
    public string Tipo { get; } = nameof(PacienteCadastradoEvent);
}