using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Orders.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId) : ICommand;
