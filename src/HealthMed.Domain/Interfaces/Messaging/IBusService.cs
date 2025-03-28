namespace HealthMed.Domain.Interfaces.Messaging;

public interface IBusService
{
    Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}