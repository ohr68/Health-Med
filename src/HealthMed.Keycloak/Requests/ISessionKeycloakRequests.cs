using HealthMed.Keycloak.Interfaces;
using Refit;

namespace HealthMed.Keycloak.Requests;

public interface ISessionKeycloakRequests : IKeycloakRequest
{
    [Post("/admin/realms/{realm}/users/{userId}/logout")]
    Task<ApiResponse<object?>> LogoutAsync(string realm, string userId, CancellationToken cancellationToken);
}