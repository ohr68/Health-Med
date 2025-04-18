using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models;

public class UserProfileAttributeMetadata
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
    [JsonPropertyName("required")]
    public bool Required { get; set; }
    [JsonPropertyName("readOnly")]
    public bool ReadOnly { get; set; }
    [JsonPropertyName("annotations")]
    public string? Annotations { get; set; }
    [JsonPropertyName("validators")]
    public string[]? Validators { get; set; }
    [JsonPropertyName("group")]
    public string? Group { get; set; }
    [JsonPropertyName("multivalued")]
    public bool Multivalued { get; set; }
}