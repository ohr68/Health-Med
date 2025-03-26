using HealthMed.Domain.Exceptions;

namespace HealthMed.Domain.Entities;

public class Especialidade : Entidade
{
    public string Nome { get; private set; }
    public virtual ICollection<Medico> Medicos { get; private set; }

    private Especialidade()
    {
    }
    
    public Especialidade(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome da especialidade não pode ser vazio.");
        
        Nome = nome;
    }
}