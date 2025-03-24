using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers.Events;

namespace Frenet.Logistic.Domain.Customers;

public sealed class Customer : Entity
{
    public Customer(
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

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }
    public Phone Phone { get; private set; }

    //Factory
    public static Customer Create(FirstName firstName, LastName lastName, Email email, Phone phone, Address address)
    {
        var customer = new Customer(Guid.NewGuid(), firstName, lastName, email, address, phone);
        
        customer.AddDomainEvent(new CustomerCreatedDomainEvent(customer.Id));
        
        return customer;
    }
}