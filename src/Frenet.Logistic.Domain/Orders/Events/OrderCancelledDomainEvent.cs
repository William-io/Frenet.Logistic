using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders.Events;

public record OrderCancelledDomainEvent(Guid OrderId) : IDomainEvent;