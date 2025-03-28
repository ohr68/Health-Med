using FluentValidation;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Mapster;
using HealthMed.Application.Services;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services
            .AddServices()
            .AddValidation()
            .ConfigureMapster();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPacienteAppService, PacienteAppService>();

        services.AddScoped<IEspecialidadeAppService, EspecialidadeAppService>();
        
        services.AddScoped<IMedicoAppService, MedicoAppService>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly));

        return services;
    }

    private static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        services.AddMapster();

        TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        return services;
    }
}