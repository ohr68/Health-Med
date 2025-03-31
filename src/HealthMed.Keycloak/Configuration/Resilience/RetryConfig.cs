using Polly;
using Polly.Retry;
using Refit;

namespace HealthMed.Keycloak.Configuration.Resilience;

public static class RetryConfig
{
    public static RetryStrategyOptions<HttpResponseMessage> GetRetryOptions()
        => new RetryStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<ApiException>()
                .HandleResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests),
            
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            MaxRetryAttempts = 4,
            Delay = TimeSpan.FromSeconds(3),
            OnRetry = static (args) =>
            {
                Console.WriteLine($"Retry attempt {args.AttemptNumber}");
                return default;
            }
        };
}