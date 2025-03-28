using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainLayer(this IServiceCollection services)
    {
        return services.AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPacienteService, PacienteService>();

        services.AddScoped<IMedicoService, MedicoService>();

        services.AddScoped<IConsultaService, ConsultaService>();

        services.AddScoped<IEspecialidadeService, EspecialidadeService>();

        return services;
    }
}