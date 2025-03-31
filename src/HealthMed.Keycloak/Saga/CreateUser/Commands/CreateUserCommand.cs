using HealthMed.Keycloak.Models;
using MediatR;

namespace HealthMed.Keycloak.Saga.CreateUser.Commands;

public class CreateUserCommand(UserRepresentation user) : IRequest<UserRepresentation>
{
    public UserRepresentation User { get; set; } = user;
}