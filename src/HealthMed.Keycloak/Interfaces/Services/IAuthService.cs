using HealthMed.Keycloak.Models.Requests;
using HealthMed.Keycloak.Models.Responses;

namespace HealthMed.Keycloak.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
}