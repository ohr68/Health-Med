using AutoMoq;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enums;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Exceptions.Consulta;
using HealthMed.Domain.Exceptions.Medico;
using HealthMed.Domain.Exceptions.Paciente;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Services;
using HealthMed.Domain.Tests.Fixture;
using Moq;

namespace HealthMed.Domain.Tests.Services;

public class ConsultaServiceTests(
    ConsultaFixture consultaFixture,
    PacienteFixture pacienteFixture,
    MedicoFixture medicoFixture)
    : IClassFixture<ConsultaFixture>, IClassFixture<PacienteFixture>, IClassFixture<MedicoFixture>
{
    private readonly AutoMoqer _mocker = new();
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Obter consulta por id com sucesso")]
    public async Task ConsultaService_ObterConsultaPorId_ComSucesso()
    {
        //Arrange
        var pacienteId = Guid.NewGuid();
        var medicoId = Guid.NewGuid();
        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(pacienteId, medicoId, horario);
        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);
        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        var consultaRetorno = await consultaService.ObterPorId(Guid.NewGuid());

        //Assert
        consultaRepositoryMock.Verify(repo => repo.ObterPorId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);
        consultaRetorno.Should().NotBeNull();
    }

    [Fact(DisplayName = "Obter consulta por id deve lançar exceção quando consulta não encontrada")]
    public async Task ConsultaService_ObterConsultaPorIdDeveLançarExcecao_QuandoConsultaNaoEncontrado()
    {
        //Arrange
        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<ConsultaNaoEncontradaException>(() => consultaService.ObterPorId(Guid.NewGuid()));
    }

    [Fact(DisplayName = "Obter consultas paciente com sucesso")]
    public async Task ConsultaService_ObterConsultasPaciente_ComSucesso()
    {
        //Arrange
        var pacienteId = Guid.NewGuid();
        var medicoId = Guid.NewGuid();
        var horario = DateTime.Now.AddHours(1);
        var consulta1 = consultaFixture.CriarConsulta(pacienteId, medicoId, horario);
        var consulta2 = consultaFixture.CriarConsulta(pacienteId, medicoId, horario);
        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        consultaRepositoryMock
            .Setup(repo => repo.ObterConsultasPaciente(pacienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([consulta1, consulta2]);
        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        var consultasPaciente = (await consultaService.ObterConsultasPaciente(pacienteId))?.ToList();

        //Assert
        consultaRepositoryMock.Verify(repo => repo.ObterConsultasPaciente(pacienteId, It.IsAny<CancellationToken>()),
            Times.Once);
        consultasPaciente.Should().NotBeNull();
        consultasPaciente.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Obter consultas medico com sucesso")]
    public async Task ConsultaService_ObterConsultasMedico_ComSucesso()
    {
        //Arrange
        var pacienteId = Guid.NewGuid();
        var medicoId = Guid.NewGuid();
        var horario = DateTime.Now.AddHours(1);
        var consulta1 = consultaFixture.CriarConsulta(pacienteId, medicoId, horario);
        var consulta2 = consultaFixture.CriarConsulta(pacienteId, medicoId, horario);
        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        consultaRepositoryMock
            .Setup(repo => repo.ObterConsultasMedico(medicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([consulta1, consulta2]);
        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        var consultasMedico = (await consultaService.ObterConsultasMedico(medicoId))?.ToList();

        //Assert
        consultaRepositoryMock.Verify(repo => repo.ObterConsultasMedico(medicoId, It.IsAny<CancellationToken>()),
            Times.Once);
        consultasMedico.Should().NotBeNull();
        consultasMedico.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Obter medicos para consultas")]
    public async Task ConsultaService_ObterMedicos_ComSucesso()
    {
        //Arrange
        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico1 = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var medico2 = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);
        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();
        medicoRepositoryMock
            .Setup(repo => repo.ObterTodos(It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([medico1, medico2]);
        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        var medicos = (await consultaService.ObterMedicos())?.ToList();

        //Assert
        medicoRepositoryMock.Verify(repo => repo.ObterTodos(It.IsAny<Guid?>(), It.IsAny<CancellationToken>()),
            Times.Once);
        medicos.Should().NotBeNull();
        medicos.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Deve poder agendar consulta quando consulta está disponível")]
    public async Task ConsultaService_DeveAgendar_QuandoConsultaDisponivel()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
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
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta,
            disponibilidadeMedico);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.PacienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);

        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.MedicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        await consultaService.Agendar(consulta);

        // Assert
        consultaRepositoryMock.Verify(repo => repo.Adicionar(consulta), Times.Once);
    }

    [Fact(DisplayName = "Não deve poder agendar consulta quando médico não está disponível")]
    public async Task ConsultaService_NaoDeveAgendar_QuandoMedicoIndisponivel()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        List<DisponibilidadeMedico> disponibilidadeMedico =
        [
            new DisponibilidadeMedico((int)DayOfWeek.Monday, 0, 1),
        ];
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta,
            disponibilidadeMedico);

        var horario = consultaFixture.ProximaSegundaAsOito();
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.PacienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);

        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.MedicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<HorarioConsultaIndisponivelException>(() => consultaService.Agendar(consulta));
    }

    [Fact(DisplayName = "Não deve poder agendar consulta quando horário consulta não está disponível")]
    public async Task ConsultaService_NaoDeveAgendar_QuandoHorarioConsultaIndisponivel()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = consultaFixture.ProximaSegundaAsOito();
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);
        var consultaMarcada = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.PacienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);

        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.MedicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(medico);

        consultaRepositoryMock
            .Setup(repo => repo.ObterConsultasPendentesMedico(consulta.MedicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([consultaMarcada]);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<HorarioConsultaIndisponivelException>(() => consultaService.Agendar(consulta));
    }

    [Fact(DisplayName = "Não deve poder agendar consulta quando paciente não encontrado")]
    public async Task ConsultaService_NaoDeveAgendar_QuandoPacienteNaoEncontrado()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = consultaFixture.ProximaSegundaAsOito();
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.PacienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<PacienteNaoEncontradoException>(() => consultaService.Agendar(consulta));
    }

    [Fact(DisplayName = "Não deve poder agendar consulta quando medico não encontrado")]
    public async Task ConsultaService_NaoDeveAgendar_QuandoMedicoNaoEncontrado()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = consultaFixture.ProximaSegundaAsOito();
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        pacienteRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.PacienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paciente);

        medicoRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.MedicoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<MedicoNaoEncontradoException>(() => consultaService.Agendar(consulta));
    }

    [Fact(DisplayName = "Deve poder cancelar consulta com sucesso.")]
    public async Task ConsultaService_Cancelar_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        await consultaService.Cancelar(consulta.Id, "Motivo cancelamento.");

        // Assert
        consulta.Status.Should().Be(StatusConsulta.Cancelada);
    }

    [Fact(DisplayName = "Não deve cancelar consulta quando já cancelada.")]
    public async Task ConsultaService_NaoDeveCancelar_QuandoJaCancelada()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);
        consulta.Cancelar("Cancelada");

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<ConsultaJaCanceladaException>(() =>
            consultaService.Cancelar(consulta.Id, "Motivo cancelamento."));
    }

    [Fact(DisplayName = "Não deve cancelar consulta quando horário ultrapassado.")]
    public async Task ConsultaService_NaoDeveCancelar_QuandoHorarioUltrapassado()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddSeconds(3);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<HorarioConsultaJaUltrapassadoException>(
            async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                await consultaService.Cancelar(consulta.Id, "Motivo cancelamento.");
            });
    }

    [Fact(DisplayName = "Não deve cancelar consulta quando justificativa não informada.")]
    public async Task ConsultaService_NaoDeveCancelar_QuandoJustificativaNaoInformada()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<DomainException>(() => consultaService.Cancelar(consulta.Id, ""));
    }

    [Fact(DisplayName = "Deve poder aceitar consulta com sucesso.")]
    public async Task ConsultaService_Aceitar_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        await consultaService.Aceitar(consulta.Id);

        // Assert
        consulta.Status.Should().Be(StatusConsulta.Aceita);
    }

    [Fact(DisplayName = "Não deve aceitar consulta quando já aceita.")]
    public async Task ConsultaService_NaoDeveAceitar_QuandoJaAceita()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);
        consulta.Aceitar();

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<ConsultaJaAceitaException>(() => consultaService.Aceitar(consulta.Id));
    }

    [Fact(DisplayName = "Não deve aceitar consulta quando cancelada.")]
    public async Task ConsultaService_NaoDeveAceitar_QuandoCancelada()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);
        consulta.Cancelar("Motivo cancelamento.");

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<ConsultaJaCanceladaException>(() => consultaService.Aceitar(consulta.Id));
    }

    [Fact(DisplayName = "Deve poder recusar consulta com sucesso.")]
    public async Task ConsultaService_Recusar_ComSucesso()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act
        await consultaService.Recusar(consulta.Id);

        // Assert
        consulta.Status.Should().Be(StatusConsulta.Recusada);
    }

    [Fact(DisplayName = "Não deve recusar consulta quando já recusada.")]
    public async Task ConsultaService_NaoDeveRecusar_QuandoJaRecusada()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);
        consulta.Recusar();

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<ConsultaJaRecusadaException>(() => consultaService.Recusar(consulta.Id));
    }

    [Fact(DisplayName = "Não deve recusar consulta quando cancelada.")]
    public async Task ConsultaService_NaoDeveRecusar_QuandoCancelada()
    {
        //Arrange
        var nomePaciente = _faker.Person.FullName;
        var emailPaciente = _faker.Person.Email;
        var cpfPaciente = _faker.Person.Cpf();
        var paciente = pacienteFixture.CriarPaciente(nomePaciente, emailPaciente, cpfPaciente);

        var nomeMedico = _faker.Name.FullName();
        var crm = "12345678/SP";
        var especialidadeId = Guid.NewGuid();
        var emailMedico = _faker.Internet.Email();
        var valorConsulta = 100;
        var medico = medicoFixture.CriarMedico(nomeMedico, crm, especialidadeId, emailMedico, valorConsulta);

        var horario = DateTime.Now.AddHours(1);
        var consulta = consultaFixture.CriarConsulta(paciente.Id, medico.Id, horario);
        consulta.Cancelar("Motivo cancelamento.");

        var consultaRepositoryMock = _mocker.GetMock<IConsultaRepository>();
        var pacienteRepositoryMock = _mocker.GetMock<IPacienteRepository>();
        var medicoRepositoryMock = _mocker.GetMock<IMedicoRepository>();

        consultaRepositoryMock
            .Setup(repo => repo.ObterPorId(consulta.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consulta);

        var consultaService = new ConsultaService(consultaRepositoryMock.Object, pacienteRepositoryMock.Object,
            medicoRepositoryMock.Object);

        //Act && Assert
        await Assert.ThrowsAsync<ConsultaJaCanceladaException>(() => consultaService.Recusar(consulta.Id));
    }
}