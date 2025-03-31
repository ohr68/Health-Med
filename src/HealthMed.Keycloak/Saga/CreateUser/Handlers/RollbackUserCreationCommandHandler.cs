using HealthMed.Keycloak.Interfaces.Services;
using HealthMed.Keycloak.Saga.CreateUser.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Keycloak.Saga.CreateUser.Handlers;

public class RollbackUserCreationCommandHandler(IUserService userService, ILogger<RollbackUserCreationCommandHandler> logger)
    : IRequestHandler<RollbackUserCreationCommand>
{
    public async Task Handle(RollbackUserCreationCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Rolling back user creation for user {UserId}", request.SagaState.UserId);

        if (request.SagaState.UserCreated)
        {
            await userService.DeleteUserAsync(request.SagaState.KeycloakUserId!, cancellationToken);
        }
    }
}