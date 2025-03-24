using Frenet.Logistic.Application.Abstractions.Clock;
using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Orders;

namespace Frenet.Logistic.Application.Orders.CancelOrder;

internal sealed class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(
        IDateTimeProvider dateTimeProvider, 
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound);
        }

        var result = order.Cancel(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
