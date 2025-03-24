using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Orders.CompleteOrder;

public sealed record CompleteOrderCommand(Guid OrderId) : ICommand;
