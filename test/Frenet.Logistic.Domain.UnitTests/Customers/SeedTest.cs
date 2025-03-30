using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Domain.UnitTests.Customers;

internal static class SeedTest
{
    public static readonly FirstName FirstName = new("John");
    public static readonly LastName LastName = new("Doe");
    public static readonly Email Email = Email.Create("john.doe@example.com").Value;
    public static readonly Phone Phone = new("5511999999999");
    public static readonly Address Address = new(
        "Montese",
        "Ceará",
        "60421010",
        "Fortaleza",
        "Teodorico");
}
