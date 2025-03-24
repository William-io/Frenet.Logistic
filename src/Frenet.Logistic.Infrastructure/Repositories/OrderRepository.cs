using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Frenet.Logistic.Infrastructure.Repositories;

internal sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    private static readonly OrderStatus[] ActiveOrderStatus =
    {
        OrderStatus.Processing,
        OrderStatus.Shipped,
        OrderStatus.Delivered
    };

    public OrderRepository(Context context) : base(context)
    {
    }

    public async Task<bool> IsOverlappingAsync(Dispatch dispatch, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Order>().AnyAsync(order =>
            order.DispatchId == dispatch.Id &&
            ActiveOrderStatus.Contains(order.Status),
            cancellationToken);
    }
}
