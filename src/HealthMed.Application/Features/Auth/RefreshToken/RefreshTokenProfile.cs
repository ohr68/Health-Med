using HealthMed.Keycloak.Models.Requests;
using HealthMed.Keycloak.Models.Responses;
using Mapster;

namespace HealthMed.Application.Features.Auth.RefreshToken;

public class RefreshTokenProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RefreshTokenCommand, RefreshTokenRequest>();
        config.NewConfig<RefreshTokenResponse, RefreshTokenResult>()
            .ConstructUsing(a => new RefreshTokenResult(a.AccessToken, TimeSpan.FromMinutes(a.ExpiresIn),
                TimeSpan.FromMinutes(a.RefreshTokenExpiresIn), a.RefreshToken, a.TokenType, a.NotValidBeforePolicy,
                a.SessionState, a.Scope));
    }
}