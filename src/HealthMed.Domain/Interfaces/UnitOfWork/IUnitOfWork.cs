namespace HealthMed.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}