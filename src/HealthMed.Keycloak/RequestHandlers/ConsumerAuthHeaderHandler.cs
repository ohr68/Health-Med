using HealthMed.Domain.Exceptions;
using HealthMed.Keycloak.Configuration;
using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models.Requests;
using Microsoft.Extensions.Options;

namespace HealthMed.Keycloak.RequestHandlers;

public class ConsumerAuthHeaderHandler(IAuthRequestHandler authRequestHandler, IAuthService authService, IOptions<KeycloakUserOptions> keycloakUser)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authData = authRequestHandler.GetAuthRequestData(GrantType.Password);
        var token = await authService.AuthenticateAsync(
            new AuthRequest
            {
                GrantType = authData.GrantType,
                ClientId = authData.ClientId,
                ClientSecret = authData.ClientSecret,
                Username = keycloakUser.Value.Username,
                Password = keycloakUser.Value.Password
            },
            cancellationToken);

        if (token == null)
            throw new BadRequestException("Falha ao autenticar");

        request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");

        return await base.SendAsync(request, cancellationToken);
    }
}