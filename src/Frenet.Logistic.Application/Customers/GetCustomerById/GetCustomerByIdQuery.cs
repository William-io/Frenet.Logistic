using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Customers.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerResponse>;