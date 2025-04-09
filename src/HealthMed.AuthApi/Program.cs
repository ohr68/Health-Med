using System.Reflection;
using HealthMed.AuthApi.Constants;
using HealthMed.AuthApi.Extensions;
using HealthMed.Common.Filters;
using HealthMed.Common.HealthChecks;
using HealthMed.Common.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

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

    builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
    builder.Services.AddOpenApi();
    builder.Services.AddPresentationLayer(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();

    builder.AddBasicHealthChecks();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Health & Med Auth Web API",
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

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

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