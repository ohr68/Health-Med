using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Entities;

public class Paciente : Entidade
{
    public string Nome { get; private set; }
    public Email Email { get; private set; }

    public Paciente(string nome, Email email)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("O nome do paciente não pode ser vazio.");

        if (string.IsNullOrEmpty(email))
            throw new DomainException("O email do paciente não pode ser vazio.");

        Nome = nome;
        Email = email;
    }

    public void Atualizar(string nome)
    {
        Nome = nome;
        DefinirAtualizacao();
    }
}