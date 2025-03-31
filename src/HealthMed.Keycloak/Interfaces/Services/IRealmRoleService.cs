using HealthMed.Keycloak.Models;

namespace HealthMed.Keycloak.Interfaces.Services;

public interface IRealmRoleService
{
    Task<RoleRepresentation?> GetRoleByNameAsync(string? roleName, CancellationToken cancellationToken);
}