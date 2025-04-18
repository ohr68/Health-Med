namespace HealthMed.Keycloak.Configuration;

public class KeycloakUserOptions
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public TimeSpan ExpireIn { get; set; }
}