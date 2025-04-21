using Bogus;
using FluentAssertions;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Tests.Fixture;

namespace HealthMed.Domain.Tests.Entities;

public class MedicoTests(MedicoFixture medicoFixture) : IClassFixture<MedicoFixture>
{
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Deve criar medico quando dado validos")]
    public void Medico_DeveCriar_QuandoValido()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;

        //Act
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

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
        var valorConsulta = 0;

        //Act && Assert
        Assert.Throws<DomainException>(() =>
            medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta));
    }
    
    [Fact(DisplayName = "Deve poder atualizar médico quando dados válidos.")]
    public void Medico_DeveAtualizarDados_QuandoDadosValidos()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var nomeMedicoAlterado = "Nome Alterado";
        var valorConsultaAlterado = 200;
        
        //Act
        medico.Atualizar(nomeMedicoAlterado, valorConsultaAlterado);
        
        // Assert
        medico.Nome.Should().Be(nomeMedicoAlterado);
        medico.ValorConsulta.Should().Be(valorConsultaAlterado);
        medico.AtualizadoEm.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Não deve atualizar paciente quando dados inválidos.")]
    public void Paciente_NaoDeveAtualizarDados_QuandoDadosInvalidos()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "123456/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var nomeMedicoAlterado = "";
        var valorConsultaAlterado = 0;
        
        //Act && Assert
        Assert.Throws<DomainException>(() => medico.Atualizar(nomeMedicoAlterado, valorConsultaAlterado));
    }
}