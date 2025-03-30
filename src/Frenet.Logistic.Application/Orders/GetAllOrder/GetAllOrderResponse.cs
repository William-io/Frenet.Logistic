namespace Frenet.Logistic.Application.Orders.GetAllOrder;

public sealed record GetAllOrderResponse(
    Guid Id,
    Guid CustomerId,
    Guid DispatchId,
    string Status,
    DateTime CreatedOnUtc,
    string ShippingName,
    string ShippingPrice);
