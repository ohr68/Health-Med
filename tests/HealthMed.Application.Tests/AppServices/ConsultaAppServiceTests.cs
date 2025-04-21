using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Tests.Fixture;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enums;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = FluentValidation.ValidationException;

namespace HealthMed.Application.Tests.AppServices;

[Collection("Tests collection")]
public class ConsultaAppServiceTests(TestsFixture fixture)
{
    private readonly Faker _faker = new Faker();

    [Fact(DisplayName = "Deve obter consulta por id com sucesso")]
    public async Task ConsultaAppService_ObterConsultaPorId_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        var consultaRepository = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
        var consulta = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(1));
        await consultaRepository.Adicionar(consulta);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        var consultaRet = await appService.ObterPorId(consulta.Id);

        //Assert
        consultaRet.Should().NotBeNull();
    }

    [Fact(DisplayName = "Deve obter consultas do paciente com sucesso")]
    public async Task ConsultaAppService_ObterConsultasPaciente_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        var consultaRepository = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
        var consulta1 = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(1));
        var consulta2 = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(2));
        await consultaRepository.Adicionar(consulta1);
        await consultaRepository.Adicionar(consulta2);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        var consultaRet = await appService.ObterConsultasPaciente(paciente.Id);

        //Assert
        consultaRet.Should().NotBeNull();
        consultaRet.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Deve obter consultas do médico com sucesso")]
    public async Task ConsultaAppService_ObterConsultasMedico_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        var consultaRepository = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
        var consulta1 = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(1));
        var consulta2 = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(2));
        await consultaRepository.Adicionar(consulta1);
        await consultaRepository.Adicionar(consulta2);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        var consultaRet = await appService.ObterConsultasMedico(medico.Id);

        //Assert
        consultaRet.Should().NotBeNull();
        consultaRet.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Deve obte médicos para agendar consulta com sucesso")]
    public async Task ConsultaAppService_ObterMedidos_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico1 = new Medico(
            _faker.Person.FullName,
            "medico1@gmail.com",
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico1);
        var medico2 = new Medico(
            _faker.Person.FullName,
            "medico2@gmail.com",
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico2);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        var medicos = await appService.ObterMedicos();

        //Assert
        medicos.Should().NotBeNull();
        medicos.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Deve agendar consulta com sucesso")]
    public async Task ConsultaAppService_Agendar_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var horarioConsulta = DateTime.Now.AddMonths(1);
        var agendarConsultaInputModel =
            new AgendarConsultaInputModel(paciente.Id, medico.Id, horarioConsulta);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        await appService.Agendar(agendarConsultaInputModel);

        //Assert
        var consultaRet = await context.Consultas.FirstOrDefaultAsync();
        consultaRet.Should().NotBeNull();
        consultaRet.PacienteId.Should().Be(paciente.Id);
        consultaRet.MedicoId.Should().Be(medico.Id);
        consultaRet.Horario.Should().Be(horarioConsulta);
        consultaRet.Valor.Should().Be(medico.ValorConsulta);
    }

    [Fact(DisplayName = "Não deve agendar consulta quando paciente inválido.")]
    public async Task ConsultaAppService_NaoDeveAgendar_QuandoPacienteInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var horarioConsulta = DateTime.Now.AddMonths(1);
        var agendarConsultaInputModel =
            new AgendarConsultaInputModel(Guid.Empty, medico.Id, horarioConsulta);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Agendar(agendarConsultaInputModel));
    }

    [Fact(DisplayName = "Não deve agendar consulta quando medico inválido.")]
    public async Task ConsultaAppService_NaoDeveAgendar_QuandoMedicoInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var horarioConsulta = DateTime.Now.AddMonths(1);
        var agendarConsultaInputModel =
            new AgendarConsultaInputModel(paciente.Id, Guid.Empty, horarioConsulta);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Agendar(agendarConsultaInputModel));
    }

    [Fact(DisplayName = "Não deve agendar consulta quando horário inválido.")]
    public async Task ConsultaAppService_NaoDeveAgendar_QuandoHorarioInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var horarioConsulta = DateTime.MinValue;
        var agendarConsultaInputModel =
            new AgendarConsultaInputModel(paciente.Id, medico.Id, horarioConsulta);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Agendar(agendarConsultaInputModel));
    }

    [Fact(DisplayName = "Deve cancelar consulta com sucesso")]
    public async Task ConsultaAppService_Cancelar_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        var consultaRepository = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
        var consulta = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(1));
        await consultaRepository.Adicionar(consulta);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var horarioConsulta = DateTime.Now.AddMonths(1);
        var justificativaCancelamento = "Motivo cancelamento";
        var cancelarConsultaInputModel =
            new CancelarConsultaInputModel(justificativaCancelamento);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        await appService.Cancelar(consulta.Id, cancelarConsultaInputModel);

        //Assert
        var consultaRet = await context.Consultas.FirstOrDefaultAsync();
        consultaRet.Should().NotBeNull();
        consultaRet.Status.Should().Be(StatusConsulta.Cancelada);
        consultaRet.JustificativaCancelamento.Should().Be(justificativaCancelamento);
    }

    [Fact(DisplayName = "Não deve cancelar consulta quando justificativa não informada")]
    public async Task ConsultaAppService_NaoDeveCancelar_QuandoJustificativaNaoInformada()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        var consultaRepository = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
        var consulta = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(1));
        await consultaRepository.Adicionar(consulta);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var justificativaCancelamento = "";
        var cancelarConsultaInputModel =
            new CancelarConsultaInputModel(justificativaCancelamento);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => appService.Cancelar(consulta.Id, cancelarConsultaInputModel));
    }
    
    [Fact(DisplayName = "Deve aceitar consulta com sucesso")]
    public async Task ConsultaAppService_Aceitar_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        var consultaRepository = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
        var consulta = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(1));
        await consultaRepository.Adicionar(consulta);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var horarioConsulta = DateTime.Now.AddMonths(1);
        var justificativaCancelamento = "Motivo cancelamento";
        var cancelarConsultaInputModel =
            new CancelarConsultaInputModel(justificativaCancelamento);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        await appService.Aceitar(consulta.Id);

        //Assert
        var consultaRet = await context.Consultas.FirstOrDefaultAsync();
        consultaRet.Should().NotBeNull();
        consultaRet.Status.Should().Be(StatusConsulta.Aceita);
    }
    
    [Fact(DisplayName = "Deve recusar consulta com sucesso")]
    public async Task ConsultaAppService_Recusar_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);

        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var medico = new Medico(
            _faker.Person.FullName,
            _faker.Person.Email,
            "123456/SP",
            especialidade.Id,
            100,
            null);
        await medicoRepository.Adicionar(medico);

        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);

        var consultaRepository = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
        var consulta = new Consulta(paciente.Id, medico.Id, DateTime.Now.AddMonths(1));
        await consultaRepository.Adicionar(consulta);

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var horarioConsulta = DateTime.Now.AddMonths(1);
        var justificativaCancelamento = "Motivo cancelamento";
        var cancelarConsultaInputModel =
            new CancelarConsultaInputModel(justificativaCancelamento);
        var appService = scope.ServiceProvider.GetRequiredService<IConsultaAppService>();

        //Act
        await appService.Recusar(consulta.Id);

        //Assert
        var consultaRet = await context.Consultas.FirstOrDefaultAsync();
        consultaRet.Should().NotBeNull();
        consultaRet.Status.Should().Be(StatusConsulta.Recusada);
    }
}