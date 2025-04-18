using HealthMed.Keycloak.Models.Dtos;

namespace HealthMed.Keycloak.Interfaces.Services;

public interface IAuthRequestHandler
{
    AuthRequestDataDto GetAuthRequestData(string grantType);
}