using FluentAssertions;
using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Customers.Events;

namespace Frenet.Logistic.Domain.UnitTests.Customers;

public class CustomerTests
{
    [Fact]
    public void Criar_deve_definir_valor_de_propriedade()
    {
        //Arrange
        var customer = Customer.Create(
            SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);

        //Assert, uso do Fluent assertionns que verifica se o valor é igual ao esperado.
        customer.FirstName.Should().Be(SeedTest.FirstName);
        customer.LastName.Should().Be(SeedTest.LastName);
        customer.Email.Should().Be(SeedTest.Email);
        customer.Phone.Should().Be(SeedTest.Phone);
        customer.Address.Should().Be(SeedTest.Address);
    }

    [Fact]
    public void Criar_Deve_Usar_AddDomainEvent_Retornando_Colecao_Somente_Leitura_Dos_Eventos()
    {
        //Arrange
        var customer = Customer.Create(
            SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);

        var eventos = customer.GetDomainEvents();
        System.Diagnostics.Debug.WriteLine($"Total de eventos: {eventos.Count}");

        foreach (var evt in eventos)
        {
            System.Diagnostics.Debug.WriteLine($"Evento: {evt.GetType().Name}");
        }

        var domainEvent = eventos.OfType<CustomerCreatedDomainEvent>().SingleOrDefault();
        System.Diagnostics.Debug.WriteLine($"ID do Cliente no Evento: {domainEvent?.CustomerId}");
        System.Diagnostics.Debug.WriteLine($"ID do Cliente na Entidade: {customer.Id}");

        domainEvent.CustomerId.Should().Be(customer.Id);
    }
}
