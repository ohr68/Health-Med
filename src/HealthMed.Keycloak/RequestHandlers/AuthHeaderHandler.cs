using Microsoft.AspNetCore.Http;

namespace HealthMed.Keycloak.RequestHandlers;

public class AuthHeaderHandler(IHttpContextAccessor contextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        
        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token.Substring("Bearer ".Length).Trim();
        
        request.Headers.Add("Authorization", $"Bearer {token}");
        
        return base.SendAsync(request, cancellationToken);
    }
}