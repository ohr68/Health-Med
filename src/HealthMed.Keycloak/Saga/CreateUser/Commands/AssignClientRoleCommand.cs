using MediatR;

namespace HealthMed.Keycloak.Saga.CreateUser.Commands;

public class AssignClientRoleCommand(string userId) : IRequest
{
    public string UserId { get; set; } = userId;
}