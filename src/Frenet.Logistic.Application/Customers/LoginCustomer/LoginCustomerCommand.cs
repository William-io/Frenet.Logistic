using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Application.Customers.LoginCustomer;

public sealed record LoginCustomerCommand(string Email) : ICommand<string>;