using HealthMed.Application.Extensions;
using HealthMed.Domain.Extensions;
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
            .AddMessagingLayer(configuration);

        return services;
    }
}