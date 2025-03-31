using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models;

public class UserConsentRepresentation
{
    [JsonPropertyName("clientId")]
    public string? ClientId { get; set; }
    [JsonPropertyName("grantedClientScopes")]
    public string[]? GrantedClientScopes { get; set; }
    [JsonPropertyName("createdDate")]
    public long CreatedDate { get; set; }
    [JsonPropertyName("lastUpdatedDate")]
    public long LastUpdatedDate { get; set; }
    [JsonPropertyName("grantedRealmRoles")]
    public string[]? GrantedRealmRoles { get; set; }
}