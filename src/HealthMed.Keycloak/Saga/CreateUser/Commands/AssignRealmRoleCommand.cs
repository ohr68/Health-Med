using MediatR;

namespace HealthMed.Keycloak.Saga.CreateUser.Commands;
public class AssignRealmRoleCommand(string userId) : IRequest
{
    public string UserId { get; set; } = userId;
}
