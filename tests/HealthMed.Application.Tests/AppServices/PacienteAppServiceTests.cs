using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Tests.Fixture;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = FluentValidation.ValidationException;

namespace HealthMed.Application.Tests.AppServices;

[Collection("Tests collection")]
public class PacienteAppServiceTests(TestsFixture fixture)
{
    private readonly Faker _faker = new Faker();

    [Fact(DisplayName = "Deve obter paciente por id com sucesso.")]
    public async Task PacienteAppService_ObterPorId_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();
        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();

        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IPacienteAppService>();

        //Act
        var pacienteRet = await appService.ObterPorId(paciente.Id);

        //Assert
        pacienteRet.Should().NotBeNull();
    }

    [Fact(DisplayName = "Deve cadastrar paciente com sucesso.")]
    public async Task PacienteAppService_Cadastrar_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var input = new CadastroPacienteInputModel(nomePaciente, emailPaciente, cpfPaciente, "Senha@123");

        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();
        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
        var appService = scope.ServiceProvider.GetRequiredService<IPacienteAppService>();

        //Act
        await appService.Cadastrar(input);

        //Assert
        var pacienteCadastrado = await pacienteRepository.ObterPorEmail(emailPaciente);
        pacienteCadastrado.Should().NotBeNull();
        pacienteCadastrado.Nome.Should().Be(nomePaciente);
    }

    [Fact(DisplayName = "Não deve cadastrar paciente quando email inválido.")]
    public async Task PacienteAppService_NaoDeveCadastrar_QuandoEmailInvalido()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = "email_invalido ";
        var cpfPaciente = _faker.Person.Cpf();
        var input = new CadastroPacienteInputModel(nomePaciente, emailPaciente, cpfPaciente, "Senha@123");

        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();
        var appService = scope.ServiceProvider.GetRequiredService<IPacienteAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Cadastrar(input));
    }

    [Fact(DisplayName = "Não deve cadastrar paciente quando nome inválido.")]
    public async Task PacienteAppService_NaoDeveCadastrar_QuandoNomeInvalido()
    {
        //Arrange
        var nomePaciente = "";
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var input = new CadastroPacienteInputModel(nomePaciente, emailPaciente, cpfPaciente, "Senha@123");

        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();
        var appService = scope.ServiceProvider.GetRequiredService<IPacienteAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Cadastrar(input));
    }

    [Fact(DisplayName = "Deve atualizar paciente com sucesso.")]
    public async Task PacienteAppService_Atualizar_ComSucesso()
    {
        //Arrange
        var nomePacienteAtualizar = "Nome atualizado";
        var input = new AtualizacaoPacienteInputModel(nomePacienteAtualizar);

        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();
        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();

        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IPacienteAppService>();

        //Act
        await appService.Atualizar(paciente.Id, input);

        //Assert
        var pacienteAtualizado = await pacienteRepository.ObterPorId(paciente.Id);
        pacienteAtualizado.Should().NotBeNull();
        pacienteAtualizado.Nome.Should().Be(nomePacienteAtualizar);
    }

    [Fact(DisplayName = "Deve atualizar paciente quando nome inválido.")]
    public async Task PacienteAppService_NaoDeveAtualizar_ComSucesso()
    {
        //Arrange
        var nomePacienteAtualizar = "";
        var input = new AtualizacaoPacienteInputModel(nomePacienteAtualizar);

        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();
        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();

        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IPacienteAppService>();

        //Act && Assert
        await Assert.ThrowsAsync<ValidationException>(() => appService.Atualizar(paciente.Id, input));
    }

    [Fact(DisplayName = "Deve excluir paciente com sucesso.")]
    public async Task PacienteAppService_Excluir_ComSucesso()
    {
        //Arrange
        using var scope = fixture.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthMedDbContext>();
        await context.Database.EnsureCreatedAsync();
        var pacienteRepository = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();

        var paciente = new Paciente(_faker.Person.FullName, _faker.Person.Email, _faker.Person.Cpf());
        await pacienteRepository.Adicionar(paciente);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var appService = scope.ServiceProvider.GetRequiredService<IPacienteAppService>();

        //Act
        await appService.Excluir(paciente.Id);

        //Assert
        var pacienteAtualizado = await pacienteRepository.ObterPorId(paciente.Id);
        pacienteAtualizado.Should().BeNull();
    }
}