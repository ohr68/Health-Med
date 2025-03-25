using HealthMed.Domain.Exceptions;
using HealthMed.Domain.ValueObjects;

namespace HealthMed.Domain.Entities;

public class Administrator : Entidade
{
    public string Nome { get; private set; }
    public Email Email { get; private set; }

    public Administrator(string nome, Email email)
    {
        if (string.IsNullOrEmpty(nome))
            throw new DomainException("O nome do admin não pode ser vazio.");

        if (string.IsNullOrEmpty(email))
            throw new DomainException("O email do admin não pode ser vazio.");
        
        Nome = nome;
        Email = email;
    }
}