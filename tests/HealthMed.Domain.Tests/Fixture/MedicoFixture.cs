using Bogus;
using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Tests.Fixture;

public class MedicoFixture
{
    public Medico CriarMedico(string nome, string crm, Guid especialidadeId, string? email = null,
        List<DisponibilidadeMedico>? disponibilidade = null)
    {
        return new Medico(nome, email ?? new Faker().Person.Email, crm, especialidadeId, disponibilidade);
    }
}