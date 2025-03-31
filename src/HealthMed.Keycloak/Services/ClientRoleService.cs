using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models;
using HealthMed.Keycloak.Requests;

namespace HealthMed.Keycloak.Services;

public class ClientRoleService(IRealmHandler realmHandler, IKeycloakClientHandler keycloakClientHandler, IClientRoleKeycloakRequests clientRoleKeycloakRequests) : IClientRoleService
{
    public async Task<RoleRepresentation?> GetRoleByNameAsync(string? roleName, CancellationToken cancellationToken)
        => await clientRoleKeycloakRequests.GetRoleByNameAsync(realmHandler.GetRealm(), keycloakClientHandler.GetClientUuid(), roleName ?? ClientRoles.DefaultUmaProtection, cancellationToken);
}