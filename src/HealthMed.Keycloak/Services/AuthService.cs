using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models.Requests;
using HealthMed.Keycloak.Models.Responses;
using HealthMed.Keycloak.Requests;

namespace HealthMed.Keycloak.Services;

public class AuthService(IRealmHandler realmHandler, IAuthKeycloakRequests authKeycloakRequests) : IAuthService
{
    public async Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken)
        => await authKeycloakRequests.LoginAsync(realmHandler.GetRealm(), request, cancellationToken);
    
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        => await authKeycloakRequests.RefreshTokenAsync(realmHandler.GetRealm(), request, cancellationToken);
}