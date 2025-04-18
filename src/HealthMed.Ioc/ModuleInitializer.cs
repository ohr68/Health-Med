using HealthMed.Application.Extensions;
using HealthMed.Caching.Extensions;
using HealthMed.Domain.Extensions;
using HealthMed.Keycloak.Extensions;
using HealthMed.Messaging.Extensions;
using HealthMed.ORM.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Ioc;

public static class ModuleInitializer
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration,
        bool isDevelopment = false)
    {
        services
            .AddDomainLayer()    
            .AddApplicationLayer()
            .AddPersistenceLayer(configuration, isDevelopment)
            .AddMessagingLayer(configuration)
            .ConfigureKeycloakIntegration(configuration)
            .AddCaching(configuration);

        return services;
    }
}