namespace Frenet.Logistic.Application.Orders.GetOrder;

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    Guid DispatchId,
    int Status,
    DateTime CreatedOnUtc,
    string ShippingName,
    string ShippingPrice);
