using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.API.Controllers.Customers;

public sealed record RegisterCustomerRequest(
    string Email,
    string FirstName,
    string LastName,
    string Phone,
    string Password,
    Address Address);

