using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders.Events;

public record OrderDeliveredDomainEvent(Guid OrderId) : IDomainEvent;