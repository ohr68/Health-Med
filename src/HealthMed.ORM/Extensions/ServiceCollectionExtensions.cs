using HealthMed.Domain.Interfaces.Repositories;
using HealthMed.ORM.Context;
using HealthMed.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.ORM.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration,
        bool isDevelopment = false)
    {
        services
            .AddDatabase(configuration, isDevelopment)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration,
        bool isDevelopment)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
                {
                    // In-memory database for testing
                    options.UseInMemoryDatabase("InMemoryDbForTesting");

                    return;
                }

                var connectionString = configuration.GetConnectionString("HealthMed")!;
                if (!isDevelopment)
                {
                    var password = Environment.GetEnvironmentVariable("SA_PASSWORD");
                    connectionString = string.Format(connectionString, password);
                }

                options.UseSqlServer(connectionString);
            });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        
        services.AddScoped<IMedicoRepository, MedicoRepository>();
        
        services.AddScoped<IConsultaRepository, ConsultaRepository>();
        
        return services;
    }
}