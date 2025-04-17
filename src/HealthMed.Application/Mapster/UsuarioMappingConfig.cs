using HealthMed.Application.Features.Auth.Login;
using HealthMed.Keycloak.Models.Requests;
using Mapster;

namespace HealthMed.Application.Mapster;

public class UsuarioMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LoginCommand, AuthRequest>()
            .ConstructUsing(src => new AuthRequest
            {
                ClientId = src.ClientId,
                ClientSecret = src.ClientSecret,
                Password = src.Senha,
                GrantType = src.GrantType,
                Username = src.Usuario,
            });
    }
}