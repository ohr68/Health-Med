using HealthMed.Domain.Exceptions;
using HealthMed.Keycloak.Enums;
using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models;
using HealthMed.Keycloak.Requests;

namespace HealthMed.Keycloak.Services;

public class UserService(
    IRealmHandler realmHandler,
    IKeycloakClientHandler keycloakClientHandler,
    IUserKeycloakRequests userKeycloakRequests) : IUserService
{
    public async Task<bool> CreateUserAsync(UserRepresentation user, CancellationToken cancellationToken)
        => (await userKeycloakRequests.CreateUserAsync(realmHandler.GetRealm(), user, cancellationToken)).IsSuccessStatusCode;

    public async Task<bool> AssignRoleAsync(string keycloakUserId, IEnumerable<RoleRepresentation> role,
        RoleType roleType,
        CancellationToken cancellationToken)
    {
        var success = roleType switch
        {
            RoleType.Client => (await userKeycloakRequests.AssignClientRoleAsync(realmHandler.GetRealm(), keycloakUserId,
                keycloakClientHandler.GetClientUuid(), role, cancellationToken)).IsSuccessStatusCode,
            RoleType.Realm => (await userKeycloakRequests.AssignRealmRoleAsync(realmHandler.GetRealm(), keycloakUserId, role,
                cancellationToken)).IsSuccessStatusCode,
            _ => throw new InvalidOperationException("Tipo de role não suportado.")
        };

        if (!success)
            throw new BadRequestException("Failed to assign role.");

        return success;
    }

    public async Task<UserRepresentation?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        => (await userKeycloakRequests.GetByUsernameAsync(realmHandler.GetRealm(), username, cancellationToken))
            .FirstOrDefault();

    public async Task<bool> UpdateUserAsync(string username, UserRepresentation user, CancellationToken cancellationToken)
    {
        var keycloakUser = await GetByUsernameAsync(username, cancellationToken)
            ?? throw new NotFoundException($"Usuário {username} não encontrado.");
        
        return (await userKeycloakRequests.UpdateUserAsync(realmHandler.GetRealm(), keycloakUser.Id!, user, cancellationToken)).IsSuccessStatusCode;
    }

    public async Task DeleteUserAsync(string keycloakUserId, CancellationToken cancellationToken)
        => await userKeycloakRequests.DeleteUserAsync(realmHandler.GetRealm(), keycloakUserId, cancellationToken);
}