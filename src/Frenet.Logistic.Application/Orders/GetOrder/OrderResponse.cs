namespace Frenet.Logistic.Application.Orders.GetOrder;

public sealed class OrderResponse
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public Guid DispatchId { get; init; }
    public int Status { get; init; }
    
    public DateTime CreatedOnUtc { get; init; }

    public string ShippingName { get; init; } = null!;
    public string ShippingPrice { get; init; } = null!;
    
}