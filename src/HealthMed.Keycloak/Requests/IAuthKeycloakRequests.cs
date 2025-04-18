using HealthMed.Keycloak.Interfaces;
using HealthMed.Keycloak.Models.Requests;
using HealthMed.Keycloak.Models.Responses;
using Refit;

namespace HealthMed.Keycloak.Requests;

public interface IAuthKeycloakRequests : IKeycloakRequest
{
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Post("/realms/{realm}/protocol/openid-connect/token")]
    Task<AuthResponse> LoginAsync(string realm, [Body(BodySerializationMethod.UrlEncoded)] AuthRequest request, CancellationToken cancellationToken);

    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Post("/realms/{realm}/protocol/openid-connect/token")]
    Task<RefreshTokenResponse> RefreshTokenAsync(string realm, [Body(BodySerializationMethod.UrlEncoded)] RefreshTokenRequest request, CancellationToken cancellationToken);
}