using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Interfaces;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Entities;

public class Paciente : Entidade, IUsuario
{
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public Guid UsuarioId { get; private set; }
    public virtual ICollection<Consulta>? Consultas { get; private set; }
    public virtual Usuario? Usuario { get; private set; }

    private Paciente()
    {
    }

    public Paciente(string nome, Email email, Cpf cpf)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("O nome não pode ser vazio.");

        if (string.IsNullOrEmpty(email))
            throw new DomainException("O email não pode ser vazio.");

        Nome = nome;
        Email = email;
        Cpf = cpf;
    }

    public void Atualizar(string nome)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("O nome não pode ser vazio.");

        Nome = nome;
        DefinirAtualizacao();
    }
}