using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Entities;

public class Especialidade
{
    public string Nome { get; private set; }

    public Especialidade(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome da especialidade não pode ser vazio.");
        
        Nome = nome;
    }
}