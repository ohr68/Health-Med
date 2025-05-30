﻿using Bogus;
using FluentAssertions;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Tests.Fixture;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.ValueObjects;
using HealthMed.ORM.Context;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = FluentValidation.ValidationException;

namespace HealthMed.Application.Tests.AppServices;

[Collection("Tests collection")]
public class MedicoAppServiceTests(TestsFixture fixture)
{
    private readonly Faker _faker = new Faker();

    [Fact(DisplayName = "Deve obter médico por id com sucesso.")]
    public async Task MedicoAppService_ObterPorId_ComSucesso()
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

        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act
        var medicoRet = await appService.ObterPorId(medico.Id);

        //Assert
        medicoRet.Should().NotBeNull();
    }

    [Fact(DisplayName = "Deve obter médico por crm com sucesso.")]
    public async Task MedicoAppService_ObterPorCrm_ComSucesso()
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

        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act
        var medicoRet = await appService.ObterPorCrm(medico.Crm);

        //Assert
        medicoRet.Should().NotBeNull();
    }

    [Fact(DisplayName = "Deve cadastrar médico com sucesso.")]
    public async Task MedicoAppService_Cadastrar_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nomeMedico = _faker.Person.FullName;
        var emailMedico = _faker.Person.Email;
        var crmMedico = new Crm("123456/SP");
        var valorConsulta = 100;
        var especialidadeId = especialidade.Id;
        var cadastroMedicoInputModel = new CadastroMedicoInputModel(
            nomeMedico,
            emailMedico,
            crmMedico,
            valorConsulta,
            especialidadeId,
            "Senha@123", null);
        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act
        await appService.Cadastrar(cadastroMedicoInputModel);

        //Assert
        var medicoCadastrado = await medicoRepository.ObterPorEmail(emailMedico);
        medicoCadastrado.Should().NotBeNull();
        medicoCadastrado.Nome.Should().Be(nomeMedico);
        medicoCadastrado.Crm.Should().Be(crmMedico);
        medicoCadastrado.ValorConsulta.Should().Be(valorConsulta);
        medicoCadastrado.EspecialidadeId.Should().Be(especialidadeId);
    }

    [Fact(DisplayName = "Não deve cadastrar médico quando email inválido.")]
    public async Task MedicoAppService_NaoDeveCadastrar_QuandoEmailInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nomeMedico = _faker.Person.FullName;
        var emailMedico = "email_invalido";
        var crmMedico = "123456/SP";
        var valorConsulta = 100;
        var especialidadeId = especialidade.Id;
        var cadastroMedicoInputModel = new CadastroMedicoInputModel(
            nomeMedico,
            emailMedico,
            crmMedico,
            valorConsulta,
            especialidadeId,
            "Senha@123", null);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Cadastrar(cadastroMedicoInputModel));
    }

    [Fact(DisplayName = "Não deve cadastrar médico quando nome inválido.")]
    public async Task MedicoAppService_NaoDeveCadastrar_QuandoNomeInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nomeMedico = "";
        var emailMedico = _faker.Person.Email;
        var crmMedico = "123456/SP";
        var valorConsulta = 100;
        var especialidadeId = especialidade.Id;
        var cadastroMedicoInputModel = new CadastroMedicoInputModel(
            nomeMedico,
            emailMedico,
            crmMedico,
            valorConsulta,
            especialidadeId,
            "Senha@123", null);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Cadastrar(cadastroMedicoInputModel));
    }

    [Fact(DisplayName = "Não deve cadastrar médico quando crm inválido.")]
    public async Task MedicoAppService_NaoDeveCadastrar_QuandoCrmInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nomeMedico = _faker.Person.FullName;
        var emailMedico = _faker.Person.Email;
        var crmMedico = "";
        var valorConsulta = 100;
        var especialidadeId = especialidade.Id;
        var cadastroMedicoInputModel = new CadastroMedicoInputModel(
            nomeMedico,
            emailMedico,
            crmMedico,
            valorConsulta,
            especialidadeId,
            "Senha@123", null);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Cadastrar(cadastroMedicoInputModel));
    }

    [Fact(DisplayName = "Não deve cadastrar médico quando valor consulta inválido.")]
    public async Task MedicoAppService_NaoDeveCadastrar_QuandoValorConsultaInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nomeMedico = _faker.Person.FullName;
        var emailMedico = _faker.Person.Email;
        var crmMedico = "12345678";
        var valorConsulta = 0;
        var especialidadeId = especialidade.Id;
        var cadastroMedicoInputModel = new CadastroMedicoInputModel(
            nomeMedico,
            emailMedico,
            crmMedico,
            valorConsulta,
            especialidadeId,
            "Senha@123", null);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Cadastrar(cadastroMedicoInputModel));
    }

    [Fact(DisplayName = "Não deve cadastrar médico quando especialidade inválida.")]
    public async Task MedicoAppService_NaoDeveCadastrar_QuandoEspecialidadeInvalida()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nomeMedico = _faker.Person.FullName;
        var emailMedico = _faker.Person.Email;
        var crmMedico = "12345678";
        var valorConsulta = 100;
        var especialidadeId = Guid.Empty;
        var cadastroMedicoInputModel = new CadastroMedicoInputModel(
            nomeMedico,
            emailMedico,
            crmMedico,
            valorConsulta,
            especialidadeId,
            "Senha@123", null);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Cadastrar(cadastroMedicoInputModel));
    }

    [Fact(DisplayName = "Deve atualizar médico com sucesso.")]
    public async Task MedicoAppService_Atualizar_ComSucesso()
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

        var nomeMedicoAlterar = _faker.Person.FullName;
        var valorConsultaAlterar = 200;
        var atualizarMedicoInputModel = new AtualizacaoMedicoInputModel(nomeMedicoAlterar, valorConsultaAlterar);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act
        await appService.Atualizar(medico.Id, atualizarMedicoInputModel);

        //Assert
        var medicoCadastrado = await medicoRepository.ObterPorId(medico.Id);
        medicoCadastrado.Should().NotBeNull();
        medicoCadastrado.Nome.Should().Be(nomeMedicoAlterar);
        medicoCadastrado.ValorConsulta.Should().Be(valorConsultaAlterar);
    }

    [Fact(DisplayName = "Não deve atualizar médico quando nome inválido.")]
    public async Task MedicoAppService_NaoDeveAtualizar_QuandoNomeInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var nomeMedico = "";
        var valorConsulta = 100;
        var atualizaoMedicoInputModel = new AtualizacaoMedicoInputModel(nomeMedico, valorConsulta);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            appService.Atualizar(Guid.Empty, atualizaoMedicoInputModel));
    }

    [Fact(DisplayName = "Não deve atualizar médico quando valor consulta inválido.")]
    public async Task MedicoAppService_NaoDeveAtualizar_QuandoValorConsultaInvalido()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var nomeMedico = _faker.Person.FullName;
        var valorConsulta = 0;
        var atualizaoMedicoInputModel = new AtualizacaoMedicoInputModel(nomeMedico, valorConsulta);
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            appService.Atualizar(Guid.Empty, atualizaoMedicoInputModel));
    }

    [Fact(DisplayName = "Deve excluir médico com sucesso.")]
    public async Task MedicoAppService_Excluir_ComSucesso()
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
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        //Act
        await appService.Excluir(medico.Id);

        //Assert
        var medicoRet = await medicoRepository.ObterPorId(medico.Id);
        medicoRet.Should().BeNull();
    }

    [Fact(DisplayName = "Deve atualizar disponibilidade médico com sucesso.")]
    public async Task MedicoAppService_AtualizarDisponibilidade_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();

        var especialidade = new Especialidade("Teste");
        context.Especialidades.Add(especialidade);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nomeMedico = _faker.Person.FullName;
        var emailMedico = _faker.Person.Email;
        var crmMedico = "123456/SP";
        var valorConsulta = 100;
        var especialidadeId = especialidade.Id;
        List<DisponibilidadeMedico> disponibilidadeMedico =
        [
            new DisponibilidadeMedico((int)DayOfWeek.Monday, 0, 23),
            new DisponibilidadeMedico((int)DayOfWeek.Tuesday, 0, 23),
            new DisponibilidadeMedico((int)DayOfWeek.Wednesday, 0, 23),
            new DisponibilidadeMedico((int)DayOfWeek.Thursday, 0, 23),
            new DisponibilidadeMedico((int)DayOfWeek.Friday, 0, 23),
            new DisponibilidadeMedico((int)DayOfWeek.Saturday, 0, 23),
            new DisponibilidadeMedico((int)DayOfWeek.Sunday, 0, 23),
        ];
        var medico = new Medico(
            nomeMedico,
            emailMedico,
            crmMedico,
            especialidadeId,
            valorConsulta,
            disponibilidadeMedico);
        var medicoRepository = scope.ServiceProvider.GetRequiredService<IMedicoRepository>();
        await medicoRepository.Adicionar(medico);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        var appService = scope.ServiceProvider.GetRequiredService<IMedicoAppService>();

        List<DisponibilidadeMedicoInputModel> disponibilidadeMedicoAlterar =
        [
            new DisponibilidadeMedicoInputModel((int)DayOfWeek.Monday, 8, 17),
            new DisponibilidadeMedicoInputModel((int)DayOfWeek.Tuesday, 8, 17),
        ];

        //Act
        await appService.AtualizarDisponibilidade(medico.Id, disponibilidadeMedicoAlterar);

        //Assert
        var medicoCadastrado = await medicoRepository.ObterPorId(medico.Id);
        medicoCadastrado!.Disponibilidade.Should().NotBeNull();
        medicoCadastrado.Disponibilidade.Should().HaveCount(2);
        medicoCadastrado.Disponibilidade.Should()
            .Match(x => x.Any(y => y.DiaSemana == (int)DayOfWeek.Monday));
        medicoCadastrado.Disponibilidade.Should()
            .Match(x => x.Any(y => y.DiaSemana == (int)DayOfWeek.Tuesday));
        medicoCadastrado.Disponibilidade.Should()
            .Match(x => x.All(y => y.DiaSemana != (int)DayOfWeek.Monday
                                   || y.DiaSemana != (int)DayOfWeek.Tuesday));
    }
}