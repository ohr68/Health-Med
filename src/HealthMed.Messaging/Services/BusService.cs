using HealthMed.Domain.Interfaces.Messaging;
using MassTransit;

namespace HealthMed.Messaging.Services;

public class BusService(IPublishEndpoint publishEndpoint) : IBusService
{
    public async Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default)
    {
        if (message != null) await publishEndpoint.Publish(message, cancellationToken);
    }
}