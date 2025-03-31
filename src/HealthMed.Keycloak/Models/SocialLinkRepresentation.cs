using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models;

public class SocialLinkRepresentation
{
    [JsonPropertyName("socialProvider")]
    public string? SocialProvider { get; set; }
    [JsonPropertyName("socialUserId")]
    public string? SocialUserId { get; set; }
    [JsonPropertyName("socialUsername")]
    public string? SocialUserName { get; set; }
}