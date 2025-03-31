using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models;

public class UserProfileMetadata
{
    [JsonPropertyName("attributes")]
    public UserProfileAttributeMetadata[]? Attributes { get; set; }
    [JsonPropertyName("groups")]
    public UserProfileAttributeGroupMetadata[]? Groups { get; set; }
}