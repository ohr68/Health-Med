using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models;
using HealthMed.Keycloak.Requests;

namespace HealthMed.Keycloak.Services;

public class RealmRoleService(IRealmHandler realmHandler, IRealmRoleKeycloakRequests realmRoleKeycloakRequests) : IRealmRoleService
{
    public async Task<RoleRepresentation?> GetRoleByNameAsync(string? roleName, CancellationToken cancellationToken)
        => await realmRoleKeycloakRequests.GetRoleByNameAsync(realmHandler.GetRealm(), roleName ?? RealmRoles.Admin, cancellationToken);
}