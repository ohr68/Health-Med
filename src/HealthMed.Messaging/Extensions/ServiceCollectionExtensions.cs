using HealthMed.Domain.Configuration;
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
        services.Configure<MassTransitSettings>(configuration.GetSection("MasstransitSettings"));
        
        services.AddMassTransit(busConfig =>
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
            {
                busConfig.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

                return;
            }
            
            busConfig.UsingRabbitMq((context, config) =>
            {
                var massTansitSettings = configuration.GetSection("MasstransitSettings").Get<MassTransitSettings>()
                                         ?? throw new InvalidOperationException(
                                             $"A chave {nameof(MassTransitSettings)} não foi encontrada ou não foi configurada corretamente.");

                config.Host(massTansitSettings.Host!, virtualHost: "/", x =>
                {
                    x.Username(massTansitSettings.User!);
                    x.Password(massTansitSettings.Password!);
                });

                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}