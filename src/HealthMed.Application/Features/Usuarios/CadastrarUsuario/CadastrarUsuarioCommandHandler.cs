using FluentValidation;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Exceptions;
using HealthMed.Domain.Interfaces.Helpers;
using HealthMed.ORM.Context;
using Mapster;
using MediatR;

namespace HealthMed.Application.Features.Usuarios.CadastrarUsuario;

public class CadastrarUsuarioCommandHandler(HealthMedDbContext context,  IPasswordHasher passwordHasher, IValidator<CadastrarUsuarioCommand> validator) : IRequestHandler<CadastrarUsuarioCommand, CadastrarUsuarioResult>
{
    public async Task<CadastrarUsuarioResult> Handle(CadastrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if(!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var usuario = request.Adapt<Usuario>();
        context.Usuarios.Add(usuario);
        usuario.Senha = passwordHasher.HashPassword(usuario.Senha!);
        
        var sucesso = await context.SaveChangesAsync(cancellationToken) > 0;

        if (!sucesso)
            throw new BadRequestException("Houve uma falha ao inserir o usuário. Tente novamente.");
        
        return new CadastrarUsuarioResult(usuario.Id, request.Senha!);
    }
}