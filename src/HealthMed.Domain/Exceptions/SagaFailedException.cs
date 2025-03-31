namespace HealthMed.Domain.Exceptions;

public class SagaFailedException: Exception
{
    public Guid SagaId { get; }
    public string? SagaName { get; }
    public string? FailedStep { get; }
    public string? Details { get; }
    
    public SagaFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    
    public SagaFailedException(Guid sagaId, string sagaName, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        SagaId = sagaId;
        SagaName = sagaName;
    }

    public SagaFailedException(Guid sagaId, string sagaName, string failedStep, string details, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        SagaId = sagaId;
        SagaName = sagaName;
        FailedStep = failedStep;
        Details = details;
    }

    public override string ToString()
    {
        var baseString = base.ToString();
        return $"{baseString}, SagaId: {SagaId}, SagaName: {SagaName}, FailedStep: {FailedStep}, Details: {Details}";
    }
}