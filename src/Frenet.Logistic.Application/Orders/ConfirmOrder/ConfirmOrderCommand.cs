using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Orders.ConfirmOrder;

public sealed record ConfirmOrderCommand(Guid OrderId) : ICommand;
