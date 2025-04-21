using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enums;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Tests.Fixture;

namespace HealthMed.Domain.Tests.Entities;

public class ConsultaTests(PacienteFixture pacienteFixture, MedicoFixture medicoFixture)
    : IClassFixture<PacienteFixture>, IClassFixture<MedicoFixture>
{
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Deve poder criar consulta quando dados válidos")]
    public void Consulta_DeveCriar_QuandoDadosValidos()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        
        var horarioConsulta = DateTime.Now.AddHours(1);
        
        //Act 
        var consulta = new Consulta(paciente.Id, medico.Id, horarioConsulta);
        
        // Assert
        consulta.MedicoId.Should().Be(medico.Id);
        consulta.PacienteId.Should().Be(paciente.Id);
        consulta.Horario.Should().Be(horarioConsulta);
        consulta.Status.Should().Be(StatusConsulta.AguardandoAceite);
    }
    
    [Fact(DisplayName = "Não deve criar consulta quando dados inválidos")]
    public void Consulta_NaoDeveCriar_QuandoDadosInvalidos()
    {
        //Arrange
        var horarioConsulta = DateTime.Now.AddHours(-3);
        
        //Act && Assert
        Assert.Throws<DomainException>(() => new Consulta(Guid.Empty, Guid.Empty, horarioConsulta));
    }
    
    [Fact(DisplayName = "Deve poder cancelar consulta")]
    public void Consulta_DeveCancelar()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        
        var horarioConsulta = DateTime.Now.AddHours(1);
        var consulta = new Consulta(paciente.Id, medico.Id, horarioConsulta);
        var justificativaCancelamento = "Motivo";
        
        //Act 
        consulta.Cancelar(justificativaCancelamento);
        
        // Assert
        consulta.Status.Should().Be(StatusConsulta.Cancelada);
        consulta.JustificativaCancelamento.Should().Be(justificativaCancelamento);
        consulta.AtualizadoEm.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Não deve cancelar consulta quando justificava não preenchida")]
    public void Consulta_NaoDeveCancelar_QuandoJustificavaNaoPreenchida()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        
        var horarioConsulta = DateTime.Now.AddHours(1);
        var consulta = new Consulta(paciente.Id, medico.Id, horarioConsulta);
        var justificativaCancelamento = "";
        
        //Act && Assert
        Assert.Throws<DomainException>(() => consulta.Cancelar(justificativaCancelamento));
    }
    
    [Fact(DisplayName = "Consulta deve poder ser aceita")]
    public void Consulta_DeveAceitar()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        
        var horarioConsulta = DateTime.Now.AddHours(1);
        var consulta = new Consulta(paciente.Id, medico.Id, horarioConsulta);
        var atualizadoEm = consulta.AtualizadoEm;
        
        //Act 
        consulta.Aceitar();
        
        // Assert
        consulta.Status.Should().Be(StatusConsulta.Aceita);
        consulta.AtualizadoEm.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Consulta deve poder ser recusada.")]
    public void Consulta_DeveRecusar()
    {
        //Arrange
        var nomePaciente = _faker.Name.FullName();
        var emailPaciente = _faker.Internet.Email();
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);
        
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        
        var horarioConsulta = DateTime.Now.AddHours(1);
        var consulta = new Consulta(paciente.Id, medico.Id, horarioConsulta);
        var atualizadoEm = consulta.AtualizadoEm;
        
        //Act 
        consulta.Recusar();
        
        // Assert
        consulta.Status.Should().Be(StatusConsulta.Recusada);
        consulta.AtualizadoEm.Should().NotBeNull();
    }
}