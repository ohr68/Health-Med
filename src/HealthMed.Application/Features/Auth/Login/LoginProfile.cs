using HealthMed.Keycloak.Models.Requests;
using HealthMed.Keycloak.Models.Responses;
using Mapster;

namespace HealthMed.Application.Features.Auth.Login;

public class LoginProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LoginCommand, AuthRequest>();
        config.NewConfig<AuthResponse, LoginResult>()
            .ConstructUsing(a => new LoginResult(a.AccessToken, TimeSpan.FromSeconds(a.ExpiresIn),
                TimeSpan.FromSeconds(a.RefreshTokenExpiresIn), a.RefreshToken, a.TokenType, a.NotValidBeforePolicy,
                a.SessionState, a.Scope));
    }
}