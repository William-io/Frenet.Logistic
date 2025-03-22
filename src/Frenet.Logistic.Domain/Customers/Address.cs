namespace Frenet.Logistic.Domain.Customers;

public record Address(
    string Country,
    string State, 
    string ZipCode,
    string City,
    string Street);