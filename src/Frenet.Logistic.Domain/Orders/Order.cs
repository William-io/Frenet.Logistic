using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders.Events;

namespace Frenet.Logistic.Domain.Orders;

public sealed class Order : Entity
{
    public Order(
        Guid id, 
        Guid dispatchId,
        Guid customerId,
        ZipCode zipCode,
        OrderStatus status,
        DateTime createdOnUtc,
        string? shippingName = null,
        string? shippingPrice = null)
        : base(id)
    {
        DispatchId = dispatchId;
        CustomerId = customerId;
        ZipCode = zipCode;
        Status = status;
        CreatedOnUtc = createdOnUtc;
        ShippingName = shippingName;
        ShippingPrice = shippingPrice;
    }
    public Guid DispatchId { get; private set; }
    public Guid CustomerId { get; private set; }
    public ZipCode ZipCode { get; private set; }
    
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ProcessingOnUtc { get; private set; }
    public DateTime ShippedOnUtc { get; private set; }
    public DateTime DeliveredOnUtc { get; private set; }
    public DateTime CancelledOnUtc { get; private set; }
    
    public OrderStatus Status { get; private set; }
    
    public string? ShippingName { get; private set; }
    public string? ShippingPrice { get; private set; }
    
    public static Order ProcessOrder(
        Dispatch dispatch,
        Guid customerId, 
        ZipCode zipCode,
        DateTime utcNow,
        ShippingPriceService shippingPriceService)
    {
        Task<ShippingPriceDetails> shippingPriceDetails = shippingPriceService.CalcularFrete(dispatch, zipCode);
        var ordering = new Order(
            Guid.NewGuid(),
            dispatch.Id,
            customerId,
            zipCode,
            OrderStatus.Processing,
            utcNow,
            shippingPriceDetails.Result.Name,
            shippingPriceDetails.Result.Price);

        ordering.AddDomainEvent(new OrderingProcessingDomainEvent(ordering.Id));
        
        dispatch.LastDispatchOnUtc = utcNow;
        
        return ordering;
    }
    
    public Result Confirm(DateTime utcNow)
    {
        if (Status != OrderStatus.Processing)
            return Result.Failure(OrderErrors.NotProcessing);

        //Caso contrario foi enviado
        Status = OrderStatus.Shipped;
        ShippedOnUtc = utcNow;

        AddDomainEvent(new OrderShippedDomainEvent(Id));

        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        if (Status != OrderStatus.Shipped)
            return Result.Failure(OrderErrors.NotShipped);
        
        Status = OrderStatus.Delivered;
        DeliveredOnUtc = utcNow;

        AddDomainEvent(new OrderDeliveredDomainEvent(Id));

        return Result.Success();
    }
    
    public Result Cancel(DateTime utcNow)
    {
        if (Status != OrderStatus.Processing)
            return Result.Failure(OrderErrors.NotProcessing);
        
        var currentDate = DateOnly.FromDateTime(utcNow);

        if (currentDate > DateOnly.FromDateTime(CreatedOnUtc).AddDays(7))
            return Result.Failure(OrderErrors.AlShipped);
        
        Status = OrderStatus.Cancelled;
        CancelledOnUtc = utcNow;
        
        AddDomainEvent(new OrderCancelledDomainEvent(Id));
        
       return Result.Success();
    }
}