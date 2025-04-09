
using HealthMed.Common.Logging;
using Microsoft.Extensions.Hosting;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting UserCreated Consumer");

            var builder = Host.CreateApplicationBuilder(args);

            builder.AddDefaultHostAppLogging();
            //builder.Services.AddServices(builder.Configuration, builder.Environment.IsDevelopment());
            
            var host = builder.Build();
            host.UseDefaultHostAppLogging();
            
            // When the app runs, it first creates the Database.
            // using var scope = host.Services.CreateScope();
            // var context = scope.ServiceProvider.GetRequiredService<MessagingDbContext>();
            // context.Database.EnsureCreated();
            
            host.Run();
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
    }
}