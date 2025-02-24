using System.Reflection;
using HealthMed.Common.HealthChecks;
using HealthMed.Common.Logging;
using HealthMed.WebApi.Filters;
using Microsoft.OpenApi.Models;
using Serilog;

namespace HealthMed.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
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

            builder.Services.AddControllers(options => { options.Filters.Add<GlobalExceptionFilter>(); });

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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Health & Med Web API V1");
                });
            }

            app.UseHttpsRedirection();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}