using FluentValidation;
using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models.Requests;
using Mapster;
using MediatR;

namespace HealthMed.Application.Features.Auth.RefreshToken;

public class RefreshTokenCommandHandler(
    IAuthService authService,
    IAuthRequestHandler authRequestHandler,
    IValidator<RefreshTokenCommand> validator) : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var (grantType, clientId, clientSecret) = authRequestHandler.GetAuthRequestData(GrantType.RefreshToken);
        request.SetAuthData(grantType, clientId, clientSecret);

        var authResponse = await authService.RefreshTokenAsync(request.Adapt<RefreshTokenRequest>(), cancellationToken);

        return authResponse.Adapt<RefreshTokenResult>();
    }
}