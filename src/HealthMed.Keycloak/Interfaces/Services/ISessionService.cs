namespace HealthMed.Keycloak.Interfaces.Services;

public interface ISessionService
{
    Task<bool> LogoutAsync(string userId, CancellationToken cancellationToken);
}