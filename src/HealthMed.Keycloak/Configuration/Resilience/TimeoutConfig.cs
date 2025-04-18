using Polly.Timeout;

namespace HealthMed.Keycloak.Configuration.Resilience;

public static class TimeoutConfig
{
    public static TimeoutStrategyOptions GetTimeoutPolicy()
        => new TimeoutStrategyOptions
        {
            Timeout = TimeSpan.FromSeconds(10),
            OnTimeout = static args =>
            {
                Console.WriteLine(
                    $"{args.Context.OperationKey}: Execution timed out after {args.Timeout.TotalSeconds} seconds.");
                return default;
            }
        };
}