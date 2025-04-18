using HealthMed.Domain.Events;
using HealthMed.Keycloak.Constants;
using HealthMed.Keycloak.Models;
using HealthMed.Keycloak.Models.Dtos;
using HealthMed.Keycloak.Saga.CreateUser;
using Mapster;

namespace HealthMed.Consumers.Profiles;

public class MedicoProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MedicoCadastradoEvent, CreateUserSagaRequest>()
            .ConstructUsing(u => new CreateUserSagaRequest(new CreateUserFlowDto(u.MedicoId, new UserRepresentation
            {
                Username = u.Crm,
                FirstName = u.Nome,
                LastName = string.Empty,
                Email = u.Email,
                Enabled = true,
                EmailVerified = true,
                Attributes = new Dictionary<string, List<string>>
                {
                    { Attributes.CustomId, new List<string> { u.MedicoId.ToString() } }
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