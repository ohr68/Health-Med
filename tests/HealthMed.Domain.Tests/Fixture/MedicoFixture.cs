using Bogus;
using HealthMed.Domain.Entities;

namespace HealthMed.Domain.Tests.Fixture;

public class MedicoFixture
{
    public Medico CriarMedico(string nome, string crm, Guid especialidadeId, string? email, decimal valorConsulta,
        List<DisponibilidadeMedico>? disponibilidade = null)
    {
        return new Medico(nome, email, crm, especialidadeId, valorConsulta, disponibilidade);
    }
}