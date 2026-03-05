using SharpTask.Domain.Interfaces;

namespace SharpTask.Infrastructure.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
