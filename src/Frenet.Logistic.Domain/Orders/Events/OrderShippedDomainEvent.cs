using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders.Events;

public sealed record OrderShippedDomainEvent(Guid OrderId) : IDomainEvent;