using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Tests.Fixture;

namespace HealthMed.Domain.Tests.Entities;

public class PacienteTests(PacienteFixture pacienteFixture) : IClassFixture<PacienteFixture>
{
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Criar criar paciente quando dados validos.")]
    public void Paciente_Criar_QuandoValido()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        
        //Act
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        
        // Assert
        paciente.Nome.Should().Be(nomePaciente);
        paciente.Email.Valor.Should().Be(emailPaciente);
    }
    
    [Fact(DisplayName = "Não deve criar paciente quando dados inválidos.")]
    public void Paciente_NaoDeveCriar_QuandoInvalido()
    {
        //Arrange
        var nomePaciente = string.Empty;
        var emailPaciente = "email_invalido";
        var cpfPaciente = string.Empty;
        
        //Act && Assert
        Assert.Throws<DomainException>(() => pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente));
    }
    
    [Fact(DisplayName = "Deve poder atualizar paciente quando dados válidos.")]
    public void Paciente_DeveAtualizarDados_QuandoDadosValidos()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        var nomePacienteAlterar = "Nome Alterado";
        
        //Act
        paciente.Atualizar(nomePacienteAlterar);
        
        // Assert
        paciente.Nome.Should().Be(nomePacienteAlterar);
        paciente.AtualizadoEm.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Não deve atualizar paciente quando dados inválidos.")]
    public void Paciente_NaoDeveAtualizarDados_QuandoDadosInvalidos()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        var nomePacienteAlterar = "";
        
        //Act && Assert
        Assert.Throws<DomainException>(() => paciente.Atualizar(nomePacienteAlterar));
    }
}