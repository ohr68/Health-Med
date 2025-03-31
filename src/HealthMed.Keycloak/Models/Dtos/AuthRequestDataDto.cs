namespace HealthMed.Keycloak.Models.Dtos;

public record AuthRequestDataDto(string GrantType, string ClientId, string ClientSecret);