using FluentValidation;
using HealthMed.Application.Extensions;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Interfaces.UnitOfWork;
using Mapster;

namespace HealthMed.Application.Services;

internal class PacienteAppService(
    IUnitOfWork uow,
    IPacienteService pacienteService,
    IBusService busService,
    IValidator<CadastroPacienteInputModel> cadastroPacienteValidation,
    IValidator<AtualizacaoPacienteInputModel> atualizacaoPacienteValidation)
    : IPacienteAppService
{
    public async Task<PacienteViewModel?> ObterPorId(Guid pacienteId, CancellationToken cancellationToken = default)
    {
        var paciente = await pacienteService.ObterPorId(pacienteId, cancellationToken);
        
        return paciente.Adapt<PacienteViewModel>();
    }

    public async Task Cadastrar(CadastroPacienteInputModel input, CancellationToken cancellationToken = default)
    {
        await cadastroPacienteValidation.ValidateAndThrowAsync(input, cancellationToken);

        var paciente = input.Adapt<Paciente>();

        await pacienteService.Cadastrar(paciente);

        await uow.CommitAsync(cancellationToken);

        await busService.DispararEventos(paciente);
    }

    public async Task Atualizar(Guid pacienteId, AtualizacaoPacienteInputModel input,
        CancellationToken cancellationToken = default)
    {
        await atualizacaoPacienteValidation.ValidateAndThrowAsync(input, cancellationToken);

        await pacienteService.Atualizar(pacienteId, input.Nome);

        await uow.CommitAsync(cancellationToken);
    }

    public async Task Excluir(Guid pacienteId, CancellationToken cancellationToken = default)
    {
        await pacienteService.Excluir(pacienteId);

        await uow.CommitAsync(cancellationToken);
    }
}