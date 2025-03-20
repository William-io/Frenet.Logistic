namespace Frenet.Logistic.Domain.Dispatchs;

public record Address(
    string Country,
    string State, 
    string ZipCode,
    string City,
    string Street);