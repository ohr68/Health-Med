using System.Reflection;
using HealthMed.AuthApi.Constants;
using Microsoft.OpenApi.Models;

namespace HealthMed.AuthApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSwagger(configuration)
            .ConfigureCors(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Health & Med Auth Web API",
                Description = ""
            });

            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter the token in the text box below.\nExample: 'your token here'"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
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
}