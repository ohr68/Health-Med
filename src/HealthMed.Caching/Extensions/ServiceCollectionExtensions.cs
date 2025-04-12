using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Caching.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var instanceName = configuration.GetSection("Redis:InstanceName")?.Value
                           ?? throw new InvalidOperationException("Chave 'Redis:InstanceName' não encontrada.");
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = instanceName;
        });

        return services;
    }
}