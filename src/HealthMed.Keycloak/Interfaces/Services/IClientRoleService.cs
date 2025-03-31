using HealthMed.Keycloak.Models;

namespace HealthMed.Keycloak.Interfaces.Services;

public interface IClientRoleService
{
    Task<RoleRepresentation?> GetRoleByNameAsync(string? roleName, CancellationToken cancellationToken);
}