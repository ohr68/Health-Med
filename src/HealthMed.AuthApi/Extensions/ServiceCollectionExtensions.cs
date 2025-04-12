using System.Reflection;
using HealthMed.AuthApi.Constants;
using HealthMed.AuthApi.Endpoints;
using Microsoft.OpenApi.Models;

namespace HealthMed.AuthApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .ConfigureCors(configuration);
        
        return services;
    }

    private static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsConfiguration.AllowHealthMedDoctorClient, policy =>
            {
                policy.WithOrigins(configuration.GetSection("AllowedClients:DoctorClientApp").Value!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            
            options.AddPolicy(CorsConfiguration.AllowHealthMedPatientClient, policy =>
            {
                policy.WithOrigins(configuration.GetSection("AllowedClients:PatientClientApp").Value!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        return services;
    }
    
    public static WebApplication AddEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        
        return app;
    }
}