using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Orders.Events;

public sealed record OrderingProcessingDomainEvent(Guid OrderId) : IDomainEvent;