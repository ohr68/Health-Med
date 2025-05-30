using HealthMed.Common.HealthChecks;
using HealthMed.Common.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Log.Information("Starting api gateway");

            var builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();
            
            builder.Services.AddProblemDetails(options =>
                options.CustomizeProblemDetails = ctx =>
                {
                    ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
                    ctx.ProblemDetails.Extensions.Add("instance",
                        $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", cfg => cfg
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
            
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            
            builder.Services.AddOcelot();
            
            builder.AddBasicHealthChecks();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            // app.UseHttpsRedirection();
            
            app.UseDefaultLogging();
            
            app.UseBasicHealthChecks();
            
            app.UseCors("AllowAny");

            await app.UseOcelot();
            
            await app.RunAsync();
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
            await Log.CloseAndFlushAsync();
        }
    }
}