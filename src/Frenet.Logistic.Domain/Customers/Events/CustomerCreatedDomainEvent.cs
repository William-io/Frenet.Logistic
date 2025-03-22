using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Customers.Events;

public record CustomerCreatedDomainEvent(Guid CustomerId) : IDomainEvent;