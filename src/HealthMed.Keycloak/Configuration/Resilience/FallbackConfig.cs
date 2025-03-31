using System.Net;
using Polly;
using Polly.Fallback;
using Refit;

namespace HealthMed.Keycloak.Configuration.Resilience;

public static class FallbackConfig
{
    public static FallbackStrategyOptions<HttpResponseMessage> GetFallbackOptions()
        => new FallbackStrategyOptions<HttpResponseMessage>
        {
            ShouldHandle =
                args => ValueTask.FromResult(args.Outcome.Exception is not null &&
                                             args.Outcome.Exception.GetType() == typeof(ApiException)),
            FallbackAction = static args =>
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Service Unavailable",
                    Status = (int)HttpStatusCode.ServiceUnavailable,
                    Detail = "The requested resource is currently unavailable. Please try again later.",
                    Type = "https://httpstatuses.com/503"
                };

                var response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(problemDetails),
                        System.Text.Encoding.UTF8, "application/problem+json")
                };

                return ValueTask.FromResult(Outcome.FromResult(response));
            },
            OnFallback = args =>
            {
                Console.WriteLine($"⚠️ Fallback triggered due to: {args.Outcome.Exception?.Message}");
                return default;
            }
        };
}