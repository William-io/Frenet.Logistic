using Frenet.Logistic.Application.Abstractions.Clock;
using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Orders;

namespace Frenet.Logistic.Application.Orders.ConfirmOrder;

internal sealed class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmOrderCommandHandler(
        IDateTimeProvider dateTimeProvider, 
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        Order? order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            return Result.Failure(OrderErrors.NotFound);

        //Apos confirmação do pedido, o status do pedido é alterado ENVIADO
        Result result = order.Confirm(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
