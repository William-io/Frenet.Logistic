using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Application.Extensions;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Orders;

namespace Frenet.Logistic.Application.Orders.GetOrder;

internal sealed class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderResponse>(new Error(
              "Order.NotFound",
              $"Pedido {request.OrderId} não encontrado!"));
        }

        return Result.Success(new OrderResponse(
            order.Id,
            order.CustomerId,
            order.DispatchId,
            order.Status.ToEnumString(),
            order.CreatedOnUtc,
            order.ShippingName,
            order.ShippingPrice));
    }
}
