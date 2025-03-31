using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models;

public class UserProfileAttributeGroupMetadata
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("displayHeader")]
    public string? DisplayHeader { get; set; }
    [JsonPropertyName("displayDescription")]
    public string? DisplayDescription { get; set; }
    [JsonPropertyName("annotations")]
    public string? Annotations { get; set; }
}