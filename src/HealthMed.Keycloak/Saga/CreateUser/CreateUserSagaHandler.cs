using HealthMed.Domain.Exceptions;
using HealthMed.Keycloak.Saga.CreateUser.Commands;
using HealthMed.Keycloak.Saga.CreateUser.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Keycloak.Saga.CreateUser;

public class CreateUserSagaHandler(IMediator mediator, ILogger<CreateUserSagaHandler> logger) : IRequestHandler<CreateUserSagaRequest>
{
    public async Task Handle(CreateUserSagaRequest request, CancellationToken cancellationToken)
    {
        var sagaState = new CreateUserSagaState { 
            UserId = request.UserDto.UserId.ToString(),
            Username = request.UserDto.User.Username
        };

        try
        {
            logger.LogInformation("Starting CreateUserFlow for user {UserId}", request.UserDto.UserId);

            var createdUser = await mediator.Send(new CreateUserCommand(request.UserDto.User), cancellationToken);
            sagaState.KeycloakUserId = createdUser.Id;
            sagaState.UserCreated = true;

            await mediator.Send(new AssignClientRoleCommand(createdUser.Id!), cancellationToken);
            sagaState.ClientRoleAssigned = true;
            
            await mediator.Send(new AssignRealmRoleCommand(createdUser.Id!), cancellationToken);
            sagaState.RealmRoleAssigned = true;

            await mediator.Publish(new CreateUserSagaCompletedEvent(sagaState.UserId), cancellationToken);
        }
        catch (Exception ex)
        {
            await mediator.Send(new RollbackUserCreationCommand(sagaState), cancellationToken);
            throw new SagaFailedException(sagaState.SagaId, nameof(CreateUserSagaHandler), "Saga failed", ex);
        }
    }
}