using System.Reflection;
using HealthMed.Common.Filters;
using HealthMed.Common.HealthChecks;
using HealthMed.Common.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using HealthMed.Ioc;
using HealthMed.ORM.Context;


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
        options.AddPolicy("AllowAny", cfg => cfg
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
    });

    builder.Services.ConfigureServices(builder.Configuration, builder.Environment.IsDevelopment());

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Health & Med Web API V1"); });

    app.UseCors("AllowAny");
    app.UseHttpsRedirection();
    app.UseBasicHealthChecks();
    app.MapControllers();

    // When the app runs, it first creates the Database.
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

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