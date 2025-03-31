using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Models;
using HealthMed.Keycloak.Saga.CreateUser.Commands;
using MediatR;

namespace HealthMed.Keycloak.Saga.CreateUser.Handlers;

public class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommand, UserRepresentation>
{
    public async Task<UserRepresentation> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var created = await userService.CreateUserAsync(request.User, cancellationToken);
        if (!created) throw new Exception($"Failed to create user {request.User.Username}");

        return await userService.GetByUsernameAsync(request.User.Username!, cancellationToken) 
               ?? throw new Exception($"Could not retrieve created user {request.User.Username}");
    }
}