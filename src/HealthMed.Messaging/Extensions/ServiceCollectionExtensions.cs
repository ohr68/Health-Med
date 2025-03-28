using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Messaging.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingLayer(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddServices()    
            .AddMassTransit(configuration);
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBusService, BusService>();

        return services;
    }
    
    private static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
            {
                config.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

                return;
            }
            
            config.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqConfig = configuration.GetSection("RabbitMq");
                var host = rabbitMqConfig["Host"]!;
                var username = rabbitMqConfig["Username"]!;
                var password = rabbitMqConfig["Password"]!;
                
                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}