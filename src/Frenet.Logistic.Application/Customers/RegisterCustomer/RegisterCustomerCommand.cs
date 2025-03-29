using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Application.Customers.RegisterCustomer;

public sealed record RegisterCustomerCommand(
    string Email, 
    string FirstName,
    string LastName,
    string Phone,
    string Password,
    Address Address) : ICommand<Guid>;