using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Requests;

namespace HealthMed.Keycloak.Services;
public class SessionService(
    IRealmHandler realmHandler,
    ISessionKeycloakRequests sessionKeycloakRequests) : ISessionService
{
    public async Task<bool> LogoutAsync(string userId, CancellationToken cancellationToken)
        => (await sessionKeycloakRequests.LogoutAsync(realmHandler.GetRealm(), userId, cancellationToken)).IsSuccessStatusCode;
}