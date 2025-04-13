using FluentValidation;
using HealthMed.Application.Extensions;
using HealthMed.Application.Features.Usuarios.CadastrarUsuario;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Interfaces.UnitOfWork;
using Mapster;
using MediatR;

namespace HealthMed.Application.Services;

internal class PacienteAppService(
    IUnitOfWork uow,
    IPacienteService pacienteService,
    IMediator mediator,
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

        var usuario = input.Adapt<CadastrarUsuarioCommand>();
        
        var usuarioCadastrado = await mediator.Send(usuario, cancellationToken);
        
        var paciente = input.Adapt<Paciente>();
        
        paciente.SetUsuario(usuarioCadastrado.Id);
        
        await pacienteService.Cadastrar(paciente, usuarioCadastrado.Senha, cancellationToken);

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