using Frenet.Logistic.Application.Abstractions.Clock;
using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;

namespace Frenet.Logistic.Application.Orders.ProcessOrder;

internal sealed class ProcessOrderCommandHandler : ICommandHandler<ProcessOrderCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDispatchRepository _dispatchRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ShippingPriceService _shippingPriceService;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public ProcessOrderCommandHandler(
        ICustomerRepository customerRepository,
        IDispatchRepository dispatchRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ShippingPriceService shippingPriceService,
        IDateTimeProvider dateTimeProvider)
    {
        _customerRepository = customerRepository;
        _dispatchRepository = dispatchRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _shippingPriceService = shippingPriceService;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result<Guid>> Handle(ProcessOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (customer is null)
            return Result.Failure<Guid>(CustomerErrors.NotFound);
        
        var dispatch = await _dispatchRepository.GetByIdAsync(request.DispatchId, cancellationToken);

        var zipCodeDefault = "01002001";
        //from & to zip code
        var zipCode = new ZipCode(zipCodeDefault, customer.Address.ZipCode);

        if (await _orderRepository.IsOverlappingAsync(dispatch, cancellationToken))
            return Result.Failure<Guid>(OrderErrors.Overlap);
        
        var order = Order.ProcessOrder(
            dispatch,
            customer.Id,
            zipCode,
            _dateTimeProvider.UtcNow,
            _shippingPriceService);
            
        _orderRepository.Add(order);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;

    }
}