using HealthMed.WebApi.Constants;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HealthMed.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .ConfigureKeycloakAuth(configuration)
            .ConfigureCors(configuration);
        
        return services;
    }
    
    private static IServiceCollection ConfigureKeycloak(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureKeycloakAuth(configuration)
            .ConfigureKeycloakAuthorization(configuration);
        
        return services;
    }

    private static IServiceCollection ConfigureKeycloakAuth(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"{configuration["Keycloak:issuer"]}/realms/{configuration["Keycloak:realm"]}",

                    ValidateAudience = true,
                    ValidAudience = "auth-api",

                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,

                    IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                    {
                        var client = new HttpClient();
                        var keyUri = $"{configuration["Keycloak:auth-server-url"]}/realms/{configuration["Keycloak:realm"]}/protocol/openid-connect/certs";
                        var response = client.GetAsync(keyUri).Result;
                        var keys = new JsonWebKeySet(response.Content.ReadAsStringAsync().Result);

                        return keys.GetSigningKeys();
                    }
                };

                options.RequireHttpsMetadata = false; // Only in develop environment
                options.SaveToken = true;
            });
        
        return services;
    }
    
    private static IServiceCollection ConfigureKeycloakAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddKeycloakAuthorization(configuration);
        
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