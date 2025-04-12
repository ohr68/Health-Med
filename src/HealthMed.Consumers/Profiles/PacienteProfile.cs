using HealthMed.Domain.Events;
using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Models;
using HealthMed.Keycloak.Models.Dtos;
using HealthMed.Keycloak.Saga.CreateUser;
using Mapster;

namespace HealthMed.Consumers.Profiles;

public class PacienteProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PacienteCadastradoEvent, CreateUserSagaRequest>()
            .ConstructUsing(u => new CreateUserSagaRequest(new CreateUserFlowDto(u.PacienteId, new UserRepresentation
            {
                Username = u.Cpf,
                FirstName = u.Nome,
                LastName = string.Empty,
                Email = u.Email,
                Enabled = true,
                EmailVerified = true,
                Attributes = new Dictionary<string, List<string>>
                {
                    { Attributes.CustomId, new List<string> { u.PacienteId.ToString() } }
                },
                Credentials = new[]
                {
                    new CredentialRepresentation
                    {
                        Type = CredentialType.Password,
                        Value = u.Senha,
                        Temporary = false
                    }
                }
            })));
    }
}