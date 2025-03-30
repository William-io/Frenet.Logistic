using Frenet.Logistic.Domain.Dispatchs;

namespace Frenet.Logistic.Domain.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsOverlappingAsync(Dispatch dispatch, CancellationToken cancellationToken = default);
    void Add(Order order);
    void Delete(Order order);

    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);
}