using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Orders.DeleteOrder;

public sealed record DeleteOrderCommand(Guid OrderId) : ICommand;
