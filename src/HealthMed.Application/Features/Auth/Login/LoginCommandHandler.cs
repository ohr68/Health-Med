using FluentValidation;
using HealthMed.Application.Interfaces.Strategy;
using HealthMed.Application.Strategy;
using HealthMed.Caching.Extensions;
using HealthMed.Domain.Enums;
using HealthMed.Domain.Exceptions;
using HealthMed.Keycloak.Configuration;
using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models.Requests;
using HealthMed.ORM.Context;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HealthMed.Application.Features.Auth.Login;

public class LoginCommandHandler(
    HealthMedDbContext context,
    IAuthRequestHandler authRequestHandler,
    IAuthService authService,
    IDistributedCache cache,
    IOptions<KeycloakUserOptions> keycloakDefaultUser,
    IValidator<LoginCommand> validator,
    IServiceProvider serviceProvider) : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly Dictionary<TipoUsuario, ILoginStrategy> _strategyMap = new()
    {
        { TipoUsuario.Medico, serviceProvider.GetRequiredService<LoginMedicoStrategy>() },
        { TipoUsuario.Paciente, serviceProvider.GetRequiredService<LoginPacienteStrategy>() }
    };
    
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (!_strategyMap.TryGetValue(request.TipoUsuario!.Value, out var strategy))
            throw new InvalidOperationException($"Nenhuma estratégia de login encontrada para tipo: {request.TipoUsuario}");

        var (valido, idUsuario) = await strategy.ValidarAsync(request, cancellationToken);

        if (!valido)
            throw new ForbiddenException($"Login não permitido para o usuário {request.Usuario}. Verifique os dados e tente novamente.");
        
        var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == idUsuario, cancellationToken)
                   ?? throw new NotFoundException($"Usuário '{request.Usuario}' não encontrado.");
        
        if (!usuario.PodeLogar())
            throw new ForbiddenException($"Login não permitido para o usuário '{request.Usuario}'.\nTente novamente mais tarde.");

        var (grantType, clientId, clientSecret) = authRequestHandler.GetAuthRequestData(GrantType.Password);
        request.SetAuthData(grantType, clientId, clientSecret);

        var authRequest = new AuthRequest()
        {
            Username = request.Usuario!.Replace("/", ""),
            Password = request.Senha,
            GrantType = grantType,
            ClientId = clientId,
            ClientSecret = clientSecret
        };
        var authResponse = await authService.AuthenticateAsync(authRequest, cancellationToken);

        Console.WriteLine($"Setting cache for user '{usuario.Id}'");
        await cache.SetRecordAsync(usuario.Id.ToString(), authResponse.AccessToken, keycloakDefaultUser.Value.ExpireIn, cancellationToken: cancellationToken);
        Console.WriteLine($"Cached successfully. Expires In {keycloakDefaultUser.Value.ExpireIn}");
        
        return authResponse.Adapt<LoginResult>();
    }
}