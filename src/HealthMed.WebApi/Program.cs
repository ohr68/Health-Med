using System.Reflection;
using HealthMed.Common.Filters;
using HealthMed.Common.HealthChecks;
using HealthMed.Common.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using HealthMed.Ioc;
using HealthMed.ORM.Context;
using HealthMed.WebApi.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    builder.AddDefaultLogging();

    builder.Services.AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
            ctx.ProblemDetails.Extensions.Add("instance",
                $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
        });

    builder.Services.AddOpenApi();
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>();
        
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        
        options.Filters.Add(new AuthorizeFilter(policy));
    });
    
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = "http://keycloak:7080/realms/master";
            options.RequireHttpsMetadata = false; 
            options.Audience = "web-api";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = "web-api",
                ValidateIssuer = true,
                ValidIssuer = "http://keycloak:7080/realms/master",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddEndpointsApiExplorer();

    builder.AddBasicHealthChecks();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Health & Med Web API",
            Description = ""
        });

        var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(CorsConfiguration.AllowHealthMedDoctorClient, cfg => cfg
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

        options.AddPolicy(CorsConfiguration.AllowHealthMedPatientClient, cfg => cfg
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
    });

    builder.Services.ConfigureServices(builder.Configuration, builder.Environment.IsDevelopment());
    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Health & Med Web API V1"); });

    app.UseCors(config =>
    {
        config.WithOrigins(CorsConfiguration.AllowHealthMedDoctorClient,
            CorsConfiguration.AllowHealthMedDoctorClient);
    });
    
    // app.UseHttpsRedirection();
    
    app.UseBasicHealthChecks();

    app.UseAuthentication();
    
    app.UseAuthorization();
    
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    Console.WriteLine($"Critical error: {ex.Message}");
    Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
    Console.WriteLine(ex.StackTrace);
}
finally
{
    Log.CloseAndFlush();
}