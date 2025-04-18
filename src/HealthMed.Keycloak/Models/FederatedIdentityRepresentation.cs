using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models;

public class FederatedIdentityRepresentation
{
    [JsonPropertyName("identityProvider")]
    public string? IdentityProvider { get; set; }
    [JsonPropertyName("userId")]
    public string? UserId { get; set; }
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}