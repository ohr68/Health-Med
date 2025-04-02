using FluentValidation;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Application.Validation;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Interfaces.UnitOfWork;
using Mapster;

namespace HealthMed.Application.Services;

internal class ConsultaAppService(
    IUnitOfWork uow,
    IConsultaService consultaService,
    AgendarConsultaInputModelValidation agendarConsultaInputModelValidation,
    CancelarConsultaInputModelValidation cancelarConsultaInputModelValidation) : IConsultaAppService
{
    public async Task<ConsultaViewModel> ObterPorId(Guid consultaId, CancellationToken cancellationToken = default)
    {
        var consulta = await consultaService.ObterPorId(consultaId, cancellationToken);

        return consulta.Adapt<ConsultaViewModel>();
    }

    public async Task<IEnumerable<ConsultaViewModel>?> ObterConsultasPaciente(Guid pacienteId,
        CancellationToken cancellationToken = default)
    {
        var consultas = await consultaService.ObterConsultasPaciente(pacienteId, cancellationToken);

        return consultas.Adapt<IEnumerable<ConsultaViewModel>>();
    }

    public async Task<IEnumerable<ConsultaViewModel>?> ObterConsultasMedico(Guid medicoId,
        CancellationToken cancellationToken = default)
    {
        var consultas = await consultaService.ObterConsultasMedico(medicoId, cancellationToken);

        return consultas.Adapt<IEnumerable<ConsultaViewModel>>();
    }
    
    public async Task<IEnumerable<ConsultaViewModel>?> ObterConsultasPendentesMedico(Guid medicoId,
        CancellationToken cancellationToken = default)
    {
        var consultas = await consultaService.ObterConsultasPendentesMedico(medicoId, cancellationToken);

        return consultas.Adapt<IEnumerable<ConsultaViewModel>>();
    }

    public async Task<IEnumerable<MedicoViewModel>?> ObterMedicos(Guid? especialidadeId = null,
        CancellationToken cancellationToken = default)
    {
        var medicos = await consultaService.ObterMedicos(especialidadeId, cancellationToken);

        return medicos.Adapt<IEnumerable<MedicoViewModel>>();
    }

    public async Task Agendar(AgendarConsultaInputModel input, CancellationToken cancellationToken = default)
    {
        await agendarConsultaInputModelValidation.ValidateAndThrowAsync(input, cancellationToken);

        var consulta = input.Adapt<Consulta>();

        await consultaService.Agendar(consulta);

        await uow.CommitAsync(cancellationToken);
    }

    public async Task Cancelar(Guid consultaId, CancelarConsultaInputModel input,
        CancellationToken cancellationToken = default)
    {
        await cancelarConsultaInputModelValidation.ValidateAndThrowAsync(input, cancellationToken);

        await consultaService.Cancelar(consultaId, input.JustificativaCancelamento);
        
        await uow.CommitAsync(cancellationToken);
    }

    public async Task Aceitar(Guid consultaId, CancellationToken cancellationToken = default)
    {
        await consultaService.Aceitar(consultaId);

        await uow.CommitAsync(cancellationToken);
    }

    public async Task Recusar(Guid consultaId, CancellationToken cancellationToken = default)
    {
        await consultaService.Recusar(consultaId);

        await uow.CommitAsync(cancellationToken);
    }
}