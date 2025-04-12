using Bogus;
using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Tests.Fixture;

public class PacienteFixture
{
    public Paciente CriarPaciente(string nome, string email, string cpf)
    {
        return new Paciente(nome, email, cpf);
    }
}