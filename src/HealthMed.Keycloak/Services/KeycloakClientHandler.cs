using HealthMed.Keycloak.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace HealthMed.Keycloak.Services;

public class KeycloakClientHandler(IConfiguration configuration) : IKeycloakClientHandler
{
    public string GetClientId()
    {
        var clientId = configuration.GetSection("KeycloakClient:Id")
                       ?? throw new InvalidOperationException("KeycloakClient:Id não encontrado.");

        if (string.IsNullOrEmpty(clientId.Value) || string.IsNullOrWhiteSpace(clientId.Value))
            throw new ArgumentException("KeycloakClient:Id não pode ser vazio ou nulo.", nameof(clientId));

        return clientId.Value;
    }

    public string GetClientUuid()
    {
        var clientUuid = configuration.GetSection("KeycloakClient:Uuid")
                         ?? throw new InvalidOperationException("KeycloakClient:Uuid não encontrado.");

        if (string.IsNullOrEmpty(clientUuid.Value) || string.IsNullOrWhiteSpace(clientUuid.Value))
            throw new ArgumentException("KeycloakClient:Uuid não pode ser vazio ou nulo.", nameof(clientUuid));

        return clientUuid.Value;
    }
}