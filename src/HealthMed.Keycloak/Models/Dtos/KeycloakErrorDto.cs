using System.Text.Json.Serialization;

namespace HealthMed.Keycloak.Models.Dtos;

public class KeycloakErrorDto
{
    [JsonPropertyName("error")]
    public string? Error  { get; set; }
    [JsonPropertyName("error_description")]
    public string? Message { get; set; }
}