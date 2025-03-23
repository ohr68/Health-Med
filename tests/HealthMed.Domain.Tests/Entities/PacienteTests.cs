using Bogus;
using FluentAssertions;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Tests.Fixture;

namespace HealthMed.Domain.Tests.Entities;

public class PacienteTests : IClassFixture<PacienteFixture>
{
    private readonly PacienteFixture _pacienteFixture;
    private readonly Faker _faker;

    public PacienteTests(PacienteFixture pacienteFixture)
    {
        _pacienteFixture = pacienteFixture;
        _faker = new Faker();
    }

    [Fact(DisplayName = "Criar paciente valido.")]
    public void Paciente_Criar_QuandoValido()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        
        //Act
        var paciente = _pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        
        // Assert
        paciente.Nome.Should().Be(nomePaciente);
        paciente.Email.Valor.Should().Be(emailPaciente);
    }
    
    [Fact(DisplayName = "Não deve criar paciente quando dados inválidos.")]
    public void Paciente_NaoDeveCriar_QuandoInvalido()
    {
        //Arrange
        var nomePaciente = "";
        var emailPaciente = "email_invalido";
        
        //Act && Assert
        Assert.Throws<DomainException>(() => _pacienteFixture.CriarPaciente(nomePaciente, emailPaciente));
    }
}