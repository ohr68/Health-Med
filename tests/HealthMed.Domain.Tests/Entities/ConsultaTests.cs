using Bogus;
using FluentAssertions;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Tests.Fixture;

namespace HealthMed.Domain.Tests.Entities;

public class ConsultaTests(PacienteFixture pacienteFixture, MedicoFixture medicoFixture)
    : IClassFixture<PacienteFixture>, IClassFixture<MedicoFixture>
{
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Deve criar consulta quando dados válidos")]
    public void Consulta_DeveCriar_QuandoDadosValidos()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico);
        
        var horarioConsulta = DateTime.Now.AddHours(1);
        
        //Act 
        var consulta = new Consulta(paciente.Id, medico.Id, horarioConsulta);
        
        // Assert
        consulta.MedicoId.Should().Be(medico.Id);
        consulta.PacienteId.Should().Be(paciente.Id);
        consulta.Horario.Should().Be(horarioConsulta);
        consulta.Cancelado.Should().Be(false);
    }
    
    [Fact(DisplayName = "Não deve criar consulta quando dados inválidos")]
    public void Consulta_NaoDeveCriar_QuandoDadosInvalidos()
    {
        //Arrange
        var horarioConsulta = DateTime.Now.AddHours(-3);
        
        //Act && Assert
        Assert.Throws<DomainException>(() => new Consulta(Guid.Empty, Guid.Empty, horarioConsulta));
    }
    
    [Fact(DisplayName = "Deve cancelar consulta")]
    public void Consulta_DeveCancelar_QuandoValido()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico);
        
        var horarioConsulta = DateTime.Now.AddHours(1);
        var consulta = new Consulta(paciente.Id, medico.Id, horarioConsulta);
        var atualizadoEm = consulta.AtualizadoEm;
        
        //Act 
        consulta.Cancelar();
        
        // Assert
        consulta.Cancelado.Should().Be(true);
        consulta.AtualizadoEm.Should().NotBeNull();
    }
}