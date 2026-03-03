namespace SharpTask.Domain.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
