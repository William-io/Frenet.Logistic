using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Application.Extensions;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Orders;

namespace Frenet.Logistic.Application.Orders.GetAllOrder;

internal sealed class GetAllOrderQueryHandler : IQueryHandler<GetAllOrderQuery, IReadOnlyList<GetAllOrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<IReadOnlyList<GetAllOrderResponse>>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);

        if (orders is null || orders.Count == 0)
        {
            return Result.Failure<IReadOnlyList<GetAllOrderResponse>>(new Error(
                "Order.NotFound",
                "Nenhum pedido encontrado!"));
        }

        var orderResponses = orders.Select(order => new GetAllOrderResponse(
            order.Id,
            order.CustomerId,
            order.DispatchId,
            order.Status.ToEnumString(),
            order.CreatedOnUtc,
            order.ShippingName,
            order.ShippingPrice));

        return Result.Success(orderResponses.ToList() as IReadOnlyList<GetAllOrderResponse>);
    }
}
