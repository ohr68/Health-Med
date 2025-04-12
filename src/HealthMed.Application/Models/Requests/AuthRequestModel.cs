using System.Text.Json.Serialization;

namespace HealthMed.Application.Models.Requests;

public abstract class AuthRequestModel
{
    [JsonIgnore]
    public string? GrantType { get; set; }
    [JsonIgnore]
    public string? ClientId { get; set; }
    [JsonIgnore]
    public string? ClientSecret { get; set; }
    
    public void SetAuthData(string grantType, string clientId, string clientSecret)
    {
        GrantType = grantType;
        ClientId = clientId;
        ClientSecret = clientSecret;
    }
}