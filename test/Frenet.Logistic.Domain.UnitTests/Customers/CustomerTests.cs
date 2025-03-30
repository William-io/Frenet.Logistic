using System.Diagnostics;
using FluentAssertions;
using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Customers.Events;
using Frenet.Logistic.Domain.UnitTests.Bases;

namespace Frenet.Logistic.Domain.UnitTests.Customers;

public class CustomerTests : BaseTesting
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
        Debug.WriteLine($"Total de eventos: {eventos.Count}");

        var domainEvent = AssertDomainEventWasExecuted<CustomerCreatedDomainEvent>(customer);

        domainEvent.CustomerId.Should().Be(customer.Id);
    }

    [Fact]
    public void Criar_Deve_Adicionar_Role_Registered_Passando_CustomerID_RoleID_Na_Tabela_CustomerRole()
    {
        var customer = Customer.Create(SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);

        customer.Roles.Should().Contain(Role.Registered); 
    }
}
