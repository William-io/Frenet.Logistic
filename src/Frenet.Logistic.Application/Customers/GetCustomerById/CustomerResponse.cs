using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Application.Customers.GetCustomerById;

public sealed record CustomerResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Phone,
    Address Address);
