using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers.Events;

namespace Frenet.Logistic.Domain.Customers;

public sealed class Customer : Entity
{
    private readonly List<Role> _roles = new();
    private Customer(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Email email,
        Address address,
        Phone phone)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
        Phone = phone;
    }

    private Customer() { }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }
    public Phone Phone { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    //Factory
    public static Customer Create(FirstName firstName, LastName lastName, Email email, Phone phone, Address address)
    {
        var customer = new Customer(Guid.NewGuid(), firstName, lastName, email, address, phone);
        
        customer.AddDomainEvent(new CustomerCreatedDomainEvent(customer.Id));

        customer._roles.Add(Role.Registered);

        return customer;
    }
}