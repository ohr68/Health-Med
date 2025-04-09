using HealthMed.Application.Extensions;
using HealthMed.Consumers.Configuration;
using HealthMed.Consumers.Consumers;
using HealthMed.Domain.Interfaces.Messaging;
using HealthMed.Keycloak.Extensions;
using HealthMed.Messaging.Services;
using HealthMed.ORM.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Consumers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        services
            .AddApplicationLayer()
            .AddPersistenceLayer(configuration, isDevelopment)
            .AddMessaging(configuration)
            .ConfigureKeycloakConsumerIntegration(configuration);

        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MassTransitSettings>(configuration.GetSection("MasstransitSettings"));

        services.AddMassTransit(busConfig =>
        {
            busConfig.AddConsumer<MedicoCadastradoConsumer>();
            busConfig.AddConsumer<MedicoAlteradoConsumer>();
            
            busConfig.UsingRabbitMq((context, config) =>
            {
                var massTansitSettings = configuration.GetSection("MasstransitSettings").Get<MassTransitSettings>()
                                         ?? throw new InvalidOperationException(
                                             $"A chave {nameof(MassTransitSettings)} não foi encontrada ou não foi configurada corretamente.");

                config.Host(massTansitSettings.Host!, "/", x =>
                {
                    x.Username(massTansitSettings.User!);
                    x.Password(massTansitSettings.Password!);
                });

                config.ConfigureEndpoints(context);
            });
        });
        
        services.AddScoped<IBusService, BusService >();

        return services;
    }
}