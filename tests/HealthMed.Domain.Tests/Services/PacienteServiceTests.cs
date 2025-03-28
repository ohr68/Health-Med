using AutoMoq;
using Bogus;
using FluentAssertions;
using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Exceptions.Paciente;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Services;
using HealthMed.Domain.Tests.Fixture;
using Moq;

namespace HealthMed.Domain.Tests.Services;

public class PacienteServiceTests(PacienteFixture pacienteFixture) : IClassFixture<PacienteFixture>
{
    private readonly AutoMoqer _mocker = new();
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Obter paciente por id com sucesso")]
    public async Task PacienteService_ObterPacientePorId_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act
        var pacienteRetorno = await pacienteService.ObterPorId(Guid.NewGuid());

        //Assert
        pacienteRepositoryMock.Verify(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
        pacienteRetorno.Should().NotBeNull();
    }

    [Fact(DisplayName = "Obter paciente por id deve lançar exceção quando paciente não encontrado")]
    public async Task PacienteService_ObterPacientePorIdDeveLançarExcecao_QuandoPacienteNaoEncontrado()
    {
        //Arrange
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<PacienteNaoEncontradoException>(() => pacienteService.ObterPorId(Guid.NewGuid()));
    }

    [Fact(DisplayName = "Cadastrar paciente com sucesso")]
    public async Task PacienteService_CadastrarPaciente_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act
        await pacienteService.Cadastrar(paciente);

        //Assert
        pacienteRepositoryMock.Verify(repo => repo.Adicionar(paciente), Times.Once);
        paciente.Eventos.Should().ContainItemsAssignableTo<PacienteCadastradoEvent>();
    }

    [Fact(DisplayName = "Cadastrar paciente deve lançar exceção quando email já está em uso.")]
    public async Task PacienteService_CadastrarPacienteDeveLancarExcecao_QuandoEmailJaEstaEmUso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);
        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorEmail(emailPaciente, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<EmailJaEstaEmUsoException>(() => pacienteService.Cadastrar(paciente));
    }

    [Fact(DisplayName = "Atualizar paciente com sucesso")]
    public async Task PacienteService_AtualizarPaciente_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        var nomePacienteAlterado = "Nome Paciente Alterado";
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(paciente.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act
        await pacienteService.Atualizar(paciente.Id, nomePacienteAlterado);

        //Assert
        pacienteRepositoryMock.Verify(repo => repo.Atualizar(paciente), Times.Once);
        paciente.Nome.Should().Be(nomePacienteAlterado);
    }

    [Fact(DisplayName = "Atualizar paciente deve lançar exceção quando paciente não encontrado")]
    public async Task PacienteService_AtualizarPacienteDeveLancarExcecao_QuandoPacienteNaoEncontrado()
    {
        //Arrange
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<PacienteNaoEncontradoException>(() =>
            pacienteService.Atualizar(Guid.NewGuid(), "Nome Paciente Alterado"));
    }

    [Fact(DisplayName = "Excluir paciente com sucesso")]
    public async Task PacienteService_ExcluirPaciente_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente);
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(paciente.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act
        await pacienteService.Excluir(paciente.Id);

        //Assert
        pacienteRepositoryMock.Verify(repo => repo.Atualizar(paciente), Times.Once);
        paciente.Apagado.Should().Be(true);
    }

    [Fact(DisplayName = "Excluir paciente deve lançar exceção quando paciente não encontrado")]
    public async Task PacienteService_ExcluirPacienteDeveLancarExcecao_QuandoPacienteNaoEncontrado()
    {
        //Arrange
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var pacienteService = new PacienteService(pacienteRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<PacienteNaoEncontradoException>(() => pacienteService.Excluir(Guid.NewGuid()));
    }
}