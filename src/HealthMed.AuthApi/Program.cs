using HealthMed.AuthApi.Constants;
using HealthMed.AuthApi.Extensions;
using HealthMed.Common.HealthChecks;
using HealthMed.Common.Logging;
using HealthMed.Ioc;
using Scalar.AspNetCore;
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

    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((document, context, _) =>
        {
            document.Info = new()
            {
                Title = "Health & Med Auth Api",
                Version = "v1",
                Description = "Utilizada para autenticar usuários dos aplicativos clientes.",
                Contact = new()
                {
                    Name = "API Support",
                    Email = "api@example.com",
                    Url = new Uri("https://api.example.com/support")
                }
            };
            return Task.CompletedTask;
        });
    });
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddPresentationLayer(builder.Configuration);
    builder.Services.ConfigureServices(builder.Configuration, builder.Environment.IsDevelopment());
    builder.Services.AddHttpContextAccessor();
    builder.AddBasicHealthChecks();

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

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Health & Med Auth Api")
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

        var https = builder.Configuration["ScalarDocs"]!;
        options.Servers = [new ScalarServer(https)];
    });
                
    // Redirect root to Scalar UI
    app.MapGet("/", () => Results.Redirect("/scalar/v1"))
        .ExcludeFromDescription();

    app.UseHttpsRedirection();
    app.UseDefaultLogging();
    app.UseGlobalExceptionHandler();
    app.AddEndpoints();
    
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