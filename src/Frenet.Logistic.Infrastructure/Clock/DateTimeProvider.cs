using Frenet.Logistic.Application.Abstractions.Clock;

namespace Frenet.Logistic.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
