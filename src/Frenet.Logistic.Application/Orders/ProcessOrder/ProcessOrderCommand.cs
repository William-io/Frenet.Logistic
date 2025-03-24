using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Orders.ProcessOrder;

public sealed record ProcessOrderCommand(
    Guid DispatchId,
    Guid CustomerId) : ICommand<Guid>;
