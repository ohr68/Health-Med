using FluentValidation;
using HealthMed.Caching.Extensions;
using HealthMed.Domain.Exceptions;
using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.ORM.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace HealthMed.Application.Features.Auth.Logout;

public class LogoutCommandHandler(
    HealthMedDbContext context,
    IDistributedCache cache,
    IJwtService jwtService,
    ISessionService sessionService,
    IValidator<LogoutCommand> validator) : IRequestHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await context.Usuarios.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                   ?? throw new NotFoundException($"Usuário '{request.UserId}' não encontrado.");

        var token = await cache.GetRecordAsync<string>(user.Id.ToString(), cancellationToken);
        var keycloakUserId = jwtService.GetClaimValueFromJwt(token, ClientScopes.UserId);

        var logoutResult = await sessionService.LogoutAsync(keycloakUserId, cancellationToken);

        if (!logoutResult)
            throw new BadRequestException("Falha ao efetuar o logout.");

        await cache.RemoveRecordAsync(user.Id.ToString(), cancellationToken);

        return new LogoutResult(logoutResult);
    }
}