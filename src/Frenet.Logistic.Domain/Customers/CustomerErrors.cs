using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Customers;

public static class CustomerErrors
{
    public static readonly Error NotFound = new(
        "Customer.Found",
        "O cliente com o identificador especificado não foi encontrado");

    public static readonly Error InvalidCredentials = new(
        "Customer.InvalidCredentials",
        "As credenciais fornecidas eram inválidas");
}