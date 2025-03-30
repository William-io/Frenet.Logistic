using System;
using System.Diagnostics;
using FluentAssertions;
using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;
using Frenet.Logistic.Domain.Orders.Events;
using Frenet.Logistic.Domain.UnitTests.Bases;
using Frenet.Logistic.Domain.UnitTests.Customers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using static Frenet.Logistic.Domain.UnitTests.Orders.ShippingPriceServiceSettings;

namespace Frenet.Logistic.Domain.UnitTests.Orders;

public class OrderTests : BaseTesting
{
    [Fact]
    public async Task Efetuar_Pedido_Deve_Executar_AddDomainEventAsync()
    {
        //Arrange
        var customer = Customer.Create(SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);

        #region Parametros de configuração do ShippingService
        var dispatchParams = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1)
        );
        var dispatch = SeedDispatchs.CreateDispatch(dispatchParams);

        var zipCode = new ZipCode("01002001", "60421010");
        var localTo_LocalFrom = SeedDispatchs.CreateZipCode(zipCode);

        var loggerMock = new Mock<ILogger<ShippingPriceService>>();

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["ShippingService:BaseUrl"]).Returns(_Settings.BaseUrl);
        configurationMock.Setup(x => x["ShippingService:ApiKey"]).Returns(_Settings.ApiKey);
        configurationMock.Setup(x => x["ShippingService:UserAgent"]).Returns(_Settings.UserAgent);

        var acessService = new ShippingPriceService(configurationMock.Object, loggerMock.Object);
        #endregion

        //Act Testando a API externa.
        var details = await acessService.CalcularFrete(dispatch, localTo_LocalFrom);

        // Update the ProcessOrder method call to match the correct number of arguments
        var order = Order.ProcessOrder(
            dispatch,
            customer.Id,
            localTo_LocalFrom,
            DateTime.UtcNow,
            acessService);

        var domainEvent = AssertDomainEventWasExecuted<OrderingProcessingDomainEvent>(order);

        domainEvent.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void Confirmar_Pedido_Deve_Mudar_Status_Para_Enviado()
    {
        // Arrange
        var customer = Customer.Create(SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);
        var currentUtcTime = DateTime.UtcNow;
        var shippingUtcTime = currentUtcTime.AddHours(1);

        var dispatchParams = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1)
        );
        var dispatch = SeedDispatchs.CreateDispatch(dispatchParams);

        var zipCode = new ZipCode("01002001", "60421010");
        var localTo_LocalFrom = SeedDispatchs.CreateZipCode(zipCode);

        var loggerMock = new Mock<ILogger<ShippingPriceService>>();
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["ShippingService:BaseUrl"]).Returns(_Settings.BaseUrl);
        configurationMock.Setup(x => x["ShippingService:ApiKey"]).Returns(_Settings.ApiKey);
        configurationMock.Setup(x => x["ShippingService:UserAgent"]).Returns(_Settings.UserAgent);

        var shippingPriceService = new ShippingPriceService(configurationMock.Object, loggerMock.Object);

        var order = Order.ProcessOrder(
            dispatch,
            customer.Id,
            localTo_LocalFrom,
            currentUtcTime,
            shippingPriceService);

        Debug.WriteLine($"Debug - Status anterior: {order.Status}");

        var result = order.Confirm(shippingUtcTime);

        Debug.WriteLine($"Debug - Status atualizado: {order.Status}");

        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Shipped);
        order.ShippedOnUtc.Should().Be(shippingUtcTime);


        var domainEvent = AssertDomainEventWasExecuted<OrderShippedDomainEvent>(order);
        domainEvent.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void Completar_Pedido_Deve_Mudar_Status_Para_Entregue()
    {
        // Arrange
        var customer = Customer.Create(SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);
        var currentUtcTime = DateTime.UtcNow;
        var shippingUtcTime = currentUtcTime.AddHours(1);
        var deliveryUtcTime = shippingUtcTime.AddHours(2);

        var dispatchParams = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1)
        );
        var dispatch = SeedDispatchs.CreateDispatch(dispatchParams);

        var zipCode = new ZipCode("01002001", "60421010");
        var localTo_LocalFrom = SeedDispatchs.CreateZipCode(zipCode);

        var loggerMock = new Mock<ILogger<ShippingPriceService>>();
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["ShippingService:BaseUrl"]).Returns(_Settings.BaseUrl);
        configurationMock.Setup(x => x["ShippingService:ApiKey"]).Returns(_Settings.ApiKey);
        configurationMock.Setup(x => x["ShippingService:UserAgent"]).Returns(_Settings.UserAgent);

        var shippingPriceService = new ShippingPriceService(configurationMock.Object, loggerMock.Object);

        // Criar e confirmar o pedido primeiro (necessário estar com status Shipped)
        var order = Order.ProcessOrder(
            dispatch,
            customer.Id,
            localTo_LocalFrom,
            currentUtcTime,
            shippingPriceService);

        var confirmResult = order.Confirm(shippingUtcTime);
        confirmResult.IsSuccess.Should().BeTrue();

        // Debug: Status antes de completar
        Debug.WriteLine($"Debug - Status antes de completar: {order.Status}");

        // Act: Completar o pedido (marcar como entregue)
        var result = order.Complete(deliveryUtcTime);

        // Debug: Status depois de completar
        Debug.WriteLine($"Debug - Status depois de completar: {order.Status}");

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Delivered);
        order.DeliveredOnUtc.Should().Be(deliveryUtcTime);

        // Verificar se o evento de domínio foi disparado
        var domainEvent = AssertDomainEventWasExecuted<OrderDeliveredDomainEvent>(order);
        domainEvent.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void Cancelar_Pedido_Deve_Mudar_Status_Para_Cancelado()
    {
        // Arrange
        var customer = Customer.Create(SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);
        var currentUtcTime = DateTime.UtcNow;
        var shippingUtcTime = currentUtcTime.AddHours(1);
        var deliveryUtcTime = shippingUtcTime.AddHours(2);
        var cancelUtcTime = deliveryUtcTime.AddDays(1); // Dentro do prazo de 7 dias após a criação

        var dispatchParams = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1)
        );
        var dispatch = SeedDispatchs.CreateDispatch(dispatchParams);

        var zipCode = new ZipCode("01002001", "60421010");
        var localTo_LocalFrom = SeedDispatchs.CreateZipCode(zipCode);

        var loggerMock = new Mock<ILogger<ShippingPriceService>>();
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["ShippingService:BaseUrl"]).Returns(_Settings.BaseUrl);
        configurationMock.Setup(x => x["ShippingService:ApiKey"]).Returns(_Settings.ApiKey);
        configurationMock.Setup(x => x["ShippingService:UserAgent"]).Returns(_Settings.UserAgent);

        var shippingPriceService = new ShippingPriceService(configurationMock.Object, loggerMock.Object);

        // Criar, confirmar e completar o pedido primeiro (necessário estar com status Delivered)
        var order = Order.ProcessOrder(
            dispatch,
            customer.Id,
            localTo_LocalFrom,
            currentUtcTime,
            shippingPriceService);

        var confirmResult = order.Confirm(shippingUtcTime);
        confirmResult.IsSuccess.Should().BeTrue();

        var completeResult = order.Complete(deliveryUtcTime);
        completeResult.IsSuccess.Should().BeTrue();

        // Debug: Status antes de cancelar
        Debug.WriteLine($"Debug - Status antes de cancelar: {order.Status}");

        // Act: Cancelar o pedido
        var result = order.Cancel(cancelUtcTime);

        // Debug: Status depois de cancelar
        Debug.WriteLine($"Debug - Status depois de cancelar: {order.Status}");

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Cancelled);
        order.CancelledOnUtc.Should().Be(cancelUtcTime);

        // Verificar se o evento de domínio foi disparado
        var domainEvent = AssertDomainEventWasExecuted<OrderCancelledDomainEvent>(order);
        domainEvent.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void Nao_Deve_Cancelar_Pedido_Se_Prazo_De_Cancelamento_Expirado()
    {
        // Arrange
        var customer = Customer.Create(SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);
        var currentUtcTime = DateTime.UtcNow;
        var shippingUtcTime = currentUtcTime.AddHours(1);
        var deliveryUtcTime = shippingUtcTime.AddHours(2);
        var cancelUtcTime = currentUtcTime.AddDays(10); // Após o prazo de 7 dias

        var dispatchParams = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1)
        );
        var dispatch = SeedDispatchs.CreateDispatch(dispatchParams);

        var zipCode = new ZipCode("01002001", "60421010");
        var localTo_LocalFrom = SeedDispatchs.CreateZipCode(zipCode);

        var loggerMock = new Mock<ILogger<ShippingPriceService>>();
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["ShippingService:BaseUrl"]).Returns(_Settings.BaseUrl);
        configurationMock.Setup(x => x["ShippingService:ApiKey"]).Returns(_Settings.ApiKey);
        configurationMock.Setup(x => x["ShippingService:UserAgent"]).Returns(_Settings.UserAgent);

        var shippingPriceService = new ShippingPriceService(configurationMock.Object, loggerMock.Object);

        // Criar, confirmar e completar o pedido primeiro (necessário estar com status Delivered)
        var order = Order.ProcessOrder(
            dispatch,
            customer.Id,
            localTo_LocalFrom,
            currentUtcTime,
            shippingPriceService);

        var confirmResult = order.Confirm(shippingUtcTime);
        confirmResult.IsSuccess.Should().BeTrue();

        var completeResult = order.Complete(deliveryUtcTime);
        completeResult.IsSuccess.Should().BeTrue();

        // Debug: Status antes da tentativa de cancelamento
        Debug.WriteLine($"Debug - Status antes da tentativa de cancelamento: {order.Status}");

        // Act: Tentar cancelar o pedido após o prazo
        var result = order.Cancel(cancelUtcTime);

        // Debug: Status depois da tentativa de cancelamento
        Debug.WriteLine($"Debug - Status depois da tentativa de cancelamento: {order.Status}");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(OrderErrors.AlShipped);
        order.Status.Should().Be(OrderStatus.Delivered); // Status não deve mudar
    }

    [Fact]
    public void Nao_Deve_Cancelar_Pedido_Com_Status_Invalido()
    {
        // Arrange
        var customer = Customer.Create(SeedTest.FirstName, SeedTest.LastName, SeedTest.Email, SeedTest.Phone, SeedTest.Address);
        var currentUtcTime = DateTime.UtcNow;
        var cancelUtcTime = currentUtcTime.AddDays(1);

        var dispatchParams = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1)
        );
        var dispatch = SeedDispatchs.CreateDispatch(dispatchParams);

        var zipCode = new ZipCode("01002001", "60421010");
        var localTo_LocalFrom = SeedDispatchs.CreateZipCode(zipCode);

        var loggerMock = new Mock<ILogger<ShippingPriceService>>();
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["ShippingService:BaseUrl"]).Returns(_Settings.BaseUrl);
        configurationMock.Setup(x => x["ShippingService:ApiKey"]).Returns(_Settings.ApiKey);
        configurationMock.Setup(x => x["ShippingService:UserAgent"]).Returns(_Settings.UserAgent);

        var shippingPriceService = new ShippingPriceService(configurationMock.Object, loggerMock.Object);

        // Criar pedido sem alterar status (permanecerá como Processing)
        var order = Order.ProcessOrder(
            dispatch,
            customer.Id,
            localTo_LocalFrom,
            currentUtcTime,
            shippingPriceService);

        // Debug: Status atual do pedido
        Debug.WriteLine($"Debug - Status atual do pedido: {order.Status}");

        // Act: Tentar cancelar o pedido com status Processing
        var result = order.Cancel(cancelUtcTime);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(OrderErrors.NotProcessing);
        order.Status.Should().Be(OrderStatus.Processing); // Status não deve mudar
    }
}

