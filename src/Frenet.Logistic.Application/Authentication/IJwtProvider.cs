using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Application.Authentication;

public interface IJwtProvider
{
    Task<string> Generate(Customer customer);
}
