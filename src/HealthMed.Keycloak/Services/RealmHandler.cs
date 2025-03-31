using HealthMed.Keycloak.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace HealthMed.Keycloak.Services;

public class RealmHandler(IConfiguration configuration) : IRealmHandler
{
    public string GetRealm()
    {
        var realm = configuration.GetSection("Keycloak:realm")
                    ?? throw new InvalidOperationException("Keycloak:realm não encontrado.");

        if (string.IsNullOrEmpty(realm.Value) || string.IsNullOrWhiteSpace(realm.Value))
            throw new ArgumentException("Keycloak:realm não pode ser vazio ou nulo.", nameof(realm));
        
        return realm.Value;
    }
}