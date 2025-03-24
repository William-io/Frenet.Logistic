namespace Frenet.Logistic.Domain.Dispatchs;

public interface IDispatchRepository
{
    Task<Dispatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}