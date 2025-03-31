using HealthMed.Keycloak.Saga.CreateUser.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMed.Keycloak.Saga.CreateUser.Handlers;

public class CreateUserSagaCompletedEventHandler(ILogger<CreateUserSagaCompletedEventHandler> logger) 
    : INotificationHandler<CreateUserSagaCompletedEvent>
{
    public Task Handle(CreateUserSagaCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("User {UserId} creation and role assignments completed successfully.", notification.UserId);
        return Task.CompletedTask;
    }
}