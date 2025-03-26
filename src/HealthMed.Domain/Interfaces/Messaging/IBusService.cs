namespace HealthMed.Domain.Interfaces.Messaging;

public interface IBusService
{
    Task<bool> Send<TMessage>(TMessage message, CancellationToken cancellationToken = default);
    Task<bool> Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}