using FluentValidation;
using HealthMed.Application.Helpers;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Interfaces.Strategy;
using HealthMed.Application.Mapster;
using HealthMed.Application.Services;
using HealthMed.Application.Strategy;
using HealthMed.Domain.Interfaces.Helpers;
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
            .AddHelpers()
            .AddStrategies()
            .ConfigureMapster();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPacienteAppService, PacienteAppService>();

        services.AddScoped<IEspecialidadeAppService, EspecialidadeAppService>();
        
        services.AddScoped<IMedicoAppService, MedicoAppService>();

        services.AddScoped<IConsultaAppService, ConsultaAppService>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly));

        return services;
    }

    private static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, CustomPasswordHasher>();

        return services;
    }

    private static IServiceCollection AddStrategies(this IServiceCollection services)
    {
        services.AddScoped<ILoginStrategy, LoginMedicoStrategy>();
        services.AddScoped<ILoginStrategy, LoginPacienteStrategy>();
        
        return services;
    }
    
    private static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        services.AddMapster();

        TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());
        TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;
        TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;

        return services;
    }

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        return services;
    }
}