using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Entities;

public class Medico : Entidade
{
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public string Crm { get; private set; }
    public Guid EspecialidadeId { get; set; }

    public virtual Especialidade Especialidade { get; private set; }

    private readonly List<DisponibilidadeMedico> _disponibilidade;

    public virtual IReadOnlyCollection<DisponibilidadeMedico> Disponibilidade => _disponibilidade.AsReadOnly();

    public Medico(string nome, string email, string crm, Guid especialidadeId,
        List<DisponibilidadeMedico>? disponibilidade)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("O nome do médico não pode ser vazio.");

        if (string.IsNullOrEmpty(email))
            throw new DomainException("O email do médico não pode ser vazio.");

        if (string.IsNullOrEmpty(crm))
            throw new DomainException("O CRM do médico não pode ser vazio.");

        if (especialidadeId == Guid.Empty)
            throw new DomainException("A especialidade do médico não pode ser vazia.");

        Nome = nome;
        Email = email;
        Crm = crm;
        EspecialidadeId = especialidadeId;

        if (disponibilidade is not null)
            _disponibilidade = disponibilidade;
    }
}