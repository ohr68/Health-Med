using Bogus;
using FluentAssertions;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Tests.Fixture;

namespace HealthMed.Domain.Tests.Entities;

public class MedicoTests : IClassFixture<MedicoFixture>
{
    private readonly MedicoFixture _medicoFixture;
    private readonly Faker _faker;

    public MedicoTests(MedicoFixture medicoFixture)
    {
        _medicoFixture = medicoFixture;
        _faker = new Faker();
    }
    
    [Fact(DisplayName = "Criar medico valido")]
    public void Medico_Criar_QuandoValido()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        
        //Act
        var medico = _medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico);
        
        // Assert
        medico.Nome.Should().Be(nomeMedico);
        medico.Crm.Should().Be(crm);
        medico.EspecialidadeId.Should().Be(especialidadeId);
        medico.Email.Valor.Should().Be(emailMedico);
    }
    
    [Fact(DisplayName = "Não deve criar médico dados inválidos")]
    public void Medico_NaoDeveCriar_QuandoNomeInvalido()
    {
        //Arrange
        var nomeMedico = "";
        var crm = "";
        var especialidadeId = Guid.Empty;
        var emailMedico = "";
        
        //Act && Assert
        Assert.Throws<DomainException>(() => _medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico));
    }
}