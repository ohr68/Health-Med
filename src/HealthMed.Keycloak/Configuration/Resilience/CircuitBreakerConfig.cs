using Polly.CircuitBreaker;

namespace HealthMed.Keycloak.Configuration.Resilience;

public static class CircuitBreakerConfig
{
    public static CircuitBreakerStrategyOptions<HttpResponseMessage> GetCircuitBreakerOptions()
    {
        return new CircuitBreakerStrategyOptions<HttpResponseMessage>
        {
            FailureRatio = 0.5, // Break if 50% of requests fail
            MinimumThroughput = 3, // At least 2 requests must be attempted before breaking
            SamplingDuration = TimeSpan.FromSeconds(60), // Monitor failures over 60 seconds
            BreakDuration = TimeSpan.FromSeconds(60), // Stay open for 60 seconds
            OnOpened = args =>
            {
                Console.WriteLine($"🚨 Circuit breaker triggered! Open for {args.BreakDuration.TotalSeconds}s.");
                return default;
            },
            OnClosed = _ =>
            {
                Console.WriteLine("✅ Circuit breaker reset! Now accepting requests.");
                return default;
            },
            OnHalfOpened = _ =>
            {
                Console.WriteLine("⚠️ Circuit breaker is half-open. Testing if service is healthy.");
                return default;
            }
        };
    }
}