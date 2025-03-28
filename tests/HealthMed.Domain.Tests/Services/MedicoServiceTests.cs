﻿using AutoMoq;
using Bogus;
using FluentAssertions;
using HealthMed.Domain.Events;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Exceptions.Medico;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Services;
using HealthMed.Domain.Tests.Fixture;
using Moq;

namespace HealthMed.Domain.Tests.Services;

public class MedicoServiceTests(MedicoFixture medicoFixture) : IClassFixture<MedicoFixture>
{
    private readonly AutoMoqer _mocker = new();
    private readonly Faker _faker = new();
    
    [Fact(DisplayName = "Obter medico por id com sucesso")]
    public async Task MedicoService_ObterMedicoPorId_ComSucesso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act
        var medicoRetorno = await medicoService.ObterPorId(Guid.NewGuid());

        //Assert
        medicoRepositoryMock.Verify(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        medicoRetorno.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Obter médico por id deve lançar exceção quando médico não encontrado")]
    public async Task MedicoService_ObterMedicoPorIdDeveLançarExcecao_QuandoMedicoNaoEncontrado()
    {
        //Arrange
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<MedicoNaoEncontradoException>(() => medicoService.ObterPorId(Guid.NewGuid()));
    }
    
    [Fact(DisplayName = "Obter medico por crm com sucesso")]
    public async Task MedicoService_ObterMedicoPorCrm_ComSucesso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorCrm(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act
        var medicoRetorno = await medicoService.ObterPorCrm(crm);

        //Assert
        medicoRepositoryMock.Verify(repo => repo.ObterPorCrm(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        medicoRetorno.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Cadastrar medico com sucesso")]
    public async Task MedicoService_CadastrarMedico_ComSucesso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act
        await medicoService.Cadastrar(medico);

        //Assert
        medicoRepositoryMock.Verify(repo => repo.Adicionar(medico), Times.Once);
        medico.Eventos.Should().ContainItemsAssignableTo<MedicoCadastradoEvent>();
    }
    
    [Fact(DisplayName = "Cadastrar medico deve lançar exceção quando email já está em uso.")]
    public async Task MedicoService_CadastrarMedicoDeveLancarExcecao_QuandoEmailJaEstaEmUso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorEmail(emailMedico, It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<EmailJaEstaEmUsoException>(() => medicoService.Cadastrar(medico));
    }
    
    [Fact(DisplayName = "Cadastrar medico deve lançar exceção quando crm já está em uso.")]
    public async Task MedicoService_CadastrarMedicoDeveLancarExcecao_QuandoCrmJaEstaEmUso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorCrm(crm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<CrmJaEstaEmUsoException>(() => medicoService.Cadastrar(medico));
    }
    
    [Fact(DisplayName = "Atualizar medico com sucesso")]
    public async Task MedicoService_AtualizarMedico_ComSucesso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var nomeMedicoAlterado = "Nome Medico Alterado";
        var valorConsultaAlterado = 200;
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(medico.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act
        await medicoService.Atualizar(medico.Id, nomeMedicoAlterado, valorConsultaAlterado);

        //Assert
        medicoRepositoryMock.Verify(repo => repo.Atualizar(medico), Times.Once);
        medico.Nome.Should().Be(nomeMedicoAlterado);
        medico.ValorConsulta.Should().Be(valorConsultaAlterado);
    }
    
    [Fact(DisplayName = "Atualizar medico deve lançar exceção quando medico não encontrado")]
    public async Task MedicoService_AtualizarMedicoDeveLancarExcecao_QuandoMedicoNaoEncontrado()
    {
        //Arrange
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<MedicoNaoEncontradoException>(() =>
            medicoService.Atualizar(Guid.NewGuid(), "Nome Medico Alterado", 100));
    }
    
    [Fact(DisplayName = "Excluir medico com sucesso")]
    public async Task MedicoService_ExcluirMedico_ComSucesso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(medico.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act
        await medicoService.Excluir(medico.Id);

        //Assert
        medicoRepositoryMock.Verify(repo => repo.Atualizar(medico), Times.Once);
        medico.Apagado.Should().Be(true);
    }
    
    [Fact(DisplayName = "Excluir medico deve lançar exceção quando medico não encontrado")]
    public async Task MedicoService_ExcluirMedicoDeveLancarExcecao_QuandoMedicoNaoEncontrado()
    {
        //Arrange
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        var medicoService = new MedicoService(medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<MedicoNaoEncontradoException>(() => medicoService.Excluir(Guid.NewGuid()));
    }
}