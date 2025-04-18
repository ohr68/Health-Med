using HealthMed.Keycloak.Interfaces;
using HealthMed.Keycloak.Models;
using Refit;

namespace HealthMed.Keycloak.Requests;

public interface IClientRoleKeycloakRequests : IKeycloakRequest
{
    [Headers("Content-Type: application/json")]
    [Get("/admin/realms/{realm}/clients/{clientId}/roles/{roleName}")]
    Task<RoleRepresentation> GetRoleByNameAsync(string realm, string clientId, string roleName, CancellationToken cancellationToken);
}