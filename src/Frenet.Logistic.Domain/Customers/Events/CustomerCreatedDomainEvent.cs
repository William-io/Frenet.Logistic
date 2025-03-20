using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Cursomers.Events;

public record CustomerCreatedDomainEvent(Guid CustomerId) : IDomainEvent;