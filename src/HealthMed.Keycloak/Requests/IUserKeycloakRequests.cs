using HealthMed.Keycloak.Interfaces;
using HealthMed.Keycloak.Models;
using Refit;

namespace HealthMed.Keycloak.Requests;

public interface IUserKeycloakRequests : IKeycloakRequest
{
    [Headers("Content-Type: application/json")]
    [Post("/admin/realms/{realm}/users")]
    Task<ApiResponse<object?>> CreateUserAsync(string realm, [Body] UserRepresentation user, CancellationToken cancellationToken);
    
    [Headers("Content-Type: application/json")]
    [Post("/admin/realms/{realm}/users/{userId}/role-mappings/clients/{clientUuid}")]
    Task<ApiResponse<object?>> AssignClientRoleAsync(string realm, string userId, string clientUuid, [Body] IEnumerable<RoleRepresentation> role, CancellationToken cancellationToken);

    [Headers("Content-Type: application/json")]
    [Post("/admin/realms/{realm}/users/{userId}/role-mappings/realm")]
    Task<ApiResponse<object?>> AssignRealmRoleAsync(string realm, string userId, [Body] IEnumerable<RoleRepresentation> role, CancellationToken cancellationToken);
    
    [Headers("Content-Type: application/json")]
    [Get("/admin/realms/{realm}/users?exact=true")]
    Task<IEnumerable<UserRepresentation>> GetByUsernameAsync(string realm, [Query] string username, CancellationToken cancellationToken);

    [Headers("Content-Type: application/json")]
    [Put("/admin/realms/{realm}/users/{userId}")]
    Task<ApiResponse<object?>> UpdateUserAsync(string realm, string userId, [Body] UserRepresentation user, CancellationToken cancellationToken);

    [Headers("Content-Type: application/json")]
    [Delete("/admin/realms/{realm}/users/{userId}")]
    Task<ApiResponse<object?>> DeleteUserAsync(string realm, string userId, CancellationToken cancellationToken);
}