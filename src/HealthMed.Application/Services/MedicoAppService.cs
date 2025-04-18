using FluentValidation;
using HealthMed.Application.Extensions;
using HealthMed.Application.Features.Usuarios.CadastrarUsuario;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Models.InputModels;
using HealthMed.Application.Models.ViewModels;
using HealthMed.Application.Validation;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Interfaces.UnitOfWork;
using Mapster;
using MediatR;

namespace HealthMed.Application.Services;

internal class MedicoAppService(
    IUnitOfWork uow,
    IMedicoService medicoService,
    IBusService busService,
    IMediator mediator,
    CadastroMedicoInputModelValidation cadastroMedicoInputModelValidation,
    AtualizacaoMedicoInputModelValidation atualizacaoMedicoInputModelValidation) : IMedicoAppService
{
    public async Task<MedicoViewModel?> ObterPorId(Guid medicoId, CancellationToken cancellationToken = default)
    {
        var medico = await medicoService.ObterPorId(medicoId, cancellationToken);

        return medico.Adapt<MedicoViewModel>();
    }

    public async Task<MedicoViewModel?> ObterPorCrm(string crm, CancellationToken cancellationToken = default)
    {
        var medico = await medicoService.ObterPorCrm(crm, cancellationToken);

        return medico?.Adapt<MedicoViewModel>();
    }

    public async Task<IEnumerable<MedicoViewModel>?> ObterTodos(CancellationToken cancellationToken = default)
    {
        var medicos = await medicoService.ObterTodos(cancellationToken);

        return medicos?.Adapt<IEnumerable<MedicoViewModel>>();
    }

    public async Task Cadastrar(CadastroMedicoInputModel input, CancellationToken cancellationToken = default)
    {
        await cadastroMedicoInputModelValidation.ValidateAndThrowAsync(input, cancellationToken);

        var usuario = input.Adapt<CadastrarUsuarioCommand>();
        
        var usuarioCadastrado = await mediator.Send(usuario, cancellationToken);
        
        var medico = input.Adapt<Medico>();
        
        medico.SetUsuario(usuarioCadastrado.Id);

        await medicoService.Cadastrar(medico, usuarioCadastrado.Senha, cancellationToken);

        await uow.CommitAsync(cancellationToken);

        await busService.DispararEventos(medico);
    }

    public async Task Atualizar(Guid medicoId, AtualizacaoMedicoInputModel input,
        CancellationToken cancellationToken = default)
    {
        await atualizacaoMedicoInputModelValidation.ValidateAndThrowAsync(input, cancellationToken);

        await medicoService.Atualizar(medicoId, input.Nome, input.ValorConsulta);

        await uow.CommitAsync(cancellationToken);
    }

    public async Task Excluir(Guid medicoId, CancellationToken cancellationToken = default)
    {
        await medicoService.Excluir(medicoId);

        await uow.CommitAsync(cancellationToken);
    }

    public async Task AtualizarDisponibilidade(Guid medicoId,
        IEnumerable<DisponibilidadeMedicoInputModel> disponibilidade, CancellationToken cancellationToken = default)
    {
        await medicoService.AtualizarDisponibilidade(medicoId,
            disponibilidade.Adapt<IEnumerable<DisponibilidadeMedico>>());

        await uow.CommitAsync(cancellationToken);
    }
}