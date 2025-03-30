using Frenet.Logistic.Application.Abstractions.Clock;
using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Orders;

namespace Frenet.Logistic.Application.Orders.DeleteOrder;

internal sealed class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IDateTimeProvider dateTimeProvider, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            return Result.Failure(OrderErrors.NotFound);

        // Verificar se foi cancelado de fato!
        var result = order.Canceled();

        if (result.IsFailure)
            return result;

        _orderRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
