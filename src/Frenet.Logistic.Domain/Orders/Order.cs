using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders;

public sealed class Order : Entity
{
    private Order(
        Guid id, 
        Guid dispatchId, 
        Guid customerId, 
        string origin,
        string destination,
        DateTime createdOnUtc,
        OrderStatus status)
        : base(id)
    {
        DispatchId = dispatchId;
        CustomerId = customerId;
        Origin = origin;
        Destination = destination;
        CreatedOnUtc = createdOnUtc;
        Status = status;
    }

    public Guid DispatchId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Origin { get; private set; }
    public string Destination { get; private set; }
    
    public DateTime CreatedOnUtc { get; private set; }
    public OrderStatus Status { get; private set; }
    
    // public static Order Ordering(
    //     Guid dispatchId, 
    //     Guid customerId, 
    //     string origin, 
    //     string destination,
    //     DateTime utcNow)
    // {
    //     
    // }
}