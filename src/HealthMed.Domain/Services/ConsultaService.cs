using HealthMed.Domain.Entities;
using HealthMed.Domain.Enums;
using HealthMed.Domain.Exceptions.Consulta;
using HealthMed.Domain.Exceptions.Medico;
using HealthMed.Domain.Exceptions.Paciente;
using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.Domain.Interfaces.Services;

namespace HealthMed.Domain.Services;

public class ConsultaService(
    IConsultaRepository consultaRepository,
    IPacienteRepository pacienteRepository,
    IMedicoRepository medicoRepository) : IConsultaService
{
    public async Task<Consulta> ObterPorId(Guid consultaId, CancellationToken cancellationToken = default)
        => await consultaRepository.ObterPorId(consultaId, cancellationToken) ??
           throw new ConsultaNaoEncontradaException();

    public async Task<IEnumerable<Consulta>?> ObterConsultasPaciente(Guid pacienteId,
        CancellationToken cancellationToken = default) =>
        await consultaRepository.ObterConsultasPaciente(pacienteId, cancellationToken);

    public async Task<IEnumerable<Consulta>?> ObterConsultasMedico(Guid medicoId,
        CancellationToken cancellationToken = default)
        => await consultaRepository.ObterConsultasMedico(medicoId, cancellationToken);

    public async Task<IEnumerable<Medico>?> ObterMedicos(Guid? especialidadeId = null,
        CancellationToken cancellationToken = default) =>
        await medicoRepository.ObterTodos(especialidadeId, cancellationToken);

    public async Task Agendar(Consulta consulta)
    {
        _ = await pacienteRepository.ObterPorId(consulta.PacienteId) ?? throw new PacienteNaoEncontradoException();

        var medico = await medicoRepository.ObterPorId(consulta.MedicoId) ?? throw new MedicoNaoEncontradoException();

        if (!medico.PossuiDisponibilidade((int)consulta.Horario.DayOfWeek, consulta.Horario.Hour))
            throw new HorarioConsultaIndisponivelException();

        var consultasMedico = await consultaRepository.ObterConsultasPendentesMedico(consulta.MedicoId);

        var horarioConsultaIndisponivel = consultasMedico?.Any(c => c.Horario == consulta.Horario) ?? false;

        if (horarioConsultaIndisponivel)
            throw new HorarioConsultaIndisponivelException();

        await consultaRepository.Adicionar(consulta);
    }

    public async Task Cancelar(Guid consultaId, string justificativaCancelamento)
    {
        var consulta = await ObterPorId(consultaId);

        if (consulta.Status == StatusConsulta.Cancelada)
            throw new ConsultaJaCanceladaException();

        if (consulta.HorarioUltrapassado)
            throw new HorarioConsultaJaUltrapassadoException();

        consulta.Cancelar(justificativaCancelamento);

        consultaRepository.Atualizar(consulta);
    }

    public async Task Aceitar(Guid consultaId)
    {
        var consulta = await ObterPorId(consultaId);

        if (consulta.Status == StatusConsulta.Aceita)
            throw new ConsultaJaAceitaException();

        if (consulta.Status == StatusConsulta.Cancelada)
            throw new ConsultaJaCanceladaException();

        consulta.Aceitar();

        consultaRepository.Atualizar(consulta);
    }

    public async Task Recusar(Guid consultaId)
    {
        var consulta = await ObterPorId(consultaId);

        if (consulta.Status == StatusConsulta.Recusada)
            throw new ConsultaJaRecusadaException();

        if (consulta.Status == StatusConsulta.Cancelada)
            throw new ConsultaJaCanceladaException();

        consulta.Recusar();

        consultaRepository.Atualizar(consulta);
    }
}