using System.Diagnostics;
using FluentAssertions;
using Frenet.Logistic.Application.Abstractions.Clock;
using Frenet.Logistic.Application.Orders.ProcessOrder;
using Frenet.Logistic.Domain.Abstractions;
using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;
using Frenet.Logistic.Domain.UnitTests.Orders;
using NSubstitute;

namespace Frenet.Logistic.Application.UnitTests.Orders;

public class ProcessOrderTests
{
    private static readonly ProcessOrderCommand Command = new(Guid.NewGuid(), Guid.NewGuid());

    private readonly ProcessOrderCommandHandler _handler;

    private readonly ICustomerRepository _customerRepositoryMock;
    private readonly IDispatchRepository _dispatchRepositoryMock;
    private readonly IOrderRepository _orderRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private readonly ShippingPriceService _shippingPriceService;
    private readonly IDateTimeProvider _dateTimeProviderMock;

    public ProcessOrderTests()
    {
        _customerRepositoryMock = Substitute.For<ICustomerRepository>();
        _dispatchRepositoryMock = Substitute.For<IDispatchRepository>();
        _orderRepositoryMock = Substitute.For<IOrderRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        _dateTimeProviderMock.UtcNow.Returns(DateTime.UtcNow);

        _handler = new ProcessOrderCommandHandler(
            _customerRepositoryMock,
            _dispatchRepositoryMock,
            _orderRepositoryMock,
            _unitOfWorkMock,
            new ShippingPriceService(),
            _dateTimeProviderMock);
    }

    [Fact]
    public async Task Handler_DeveRetornarFalha_QuandoClienteForNulo()
    {
        _customerRepositoryMock.GetByIdAsync(Command.CustomerId, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var result = await _handler.Handle(Command, default);
        // Debug: Exibir o valor do cliente
        Debug.WriteLine($"Debug - Cliente retornado: {_customerRepositoryMock.GetByIdAsync(Command.CustomerId, Arg.Any<CancellationToken>()).Result}");

        result.Error.Should().Be(CustomerErrors.NotFound);
    }

    [Fact]
    public async Task Handler_DeveRetonarFalha_Quando_UnitOfWork_Lancar_Excecao()
    {
        // Arrange
        var customer = Customer.Create(
            SeedTest.FirstName,
            SeedTest.LastName,
            SeedTest.Email,
            SeedTest.Phone,
            SeedTest.Address);

        var dispatch = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1));

        // Adicionar o SeedDispatchs.CreateDispatch que provavelmente configura melhor o objeto dispatch
        dispatch = SeedDispatchs.CreateDispatch(dispatch);

        // Configurar os mocks para retornar valores válidos
        _customerRepositoryMock.GetByIdAsync(Command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _dispatchRepositoryMock.GetByIdAsync(Command.DispatchId, Arg.Any<CancellationToken>())
            .Returns(dispatch);

        _orderRepositoryMock.IsOverlappingAsync(Arg.Any<Dispatch>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Criar uma classe fake do ShippingPriceService
        var fakeShippingService = new TestShippingPriceService();

        // Reconstruir o handler com o serviço fake
        var handlerWithFakeService = new ProcessOrderCommandHandler(
            _customerRepositoryMock,
            _dispatchRepositoryMock,
            _orderRepositoryMock,
            _unitOfWorkMock,
            fakeShippingService,
            _dateTimeProviderMock);

        // Act
        var result = await handlerWithFakeService.Handle(Command, default);

        // Debug: Exibir o resultado
        Debug.WriteLine($"Debug - Resultado: IsSuccess={result.IsFailure}, Error={result.Error}");

        // Assert
        result.Error.Should().Be(OrderErrors.Overlap);
    }

    [Fact]
    public async Task Handler_DeveRetonarSucesso_Quando_Pedido_For_Processado()
    {
        // Arrange
        var customer = Customer.Create(
            SeedTest.FirstName,
            SeedTest.LastName,
            SeedTest.Email,
            SeedTest.Phone,
            SeedTest.Address);

        var dispatch = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1));

        // Adicionar o SeedDispatchs.CreateDispatch que provavelmente configura melhor o objeto dispatch
        dispatch = SeedDispatchs.CreateDispatch(dispatch);

        // Configurar os mocks para retornar valores válidos
        _customerRepositoryMock.GetByIdAsync(Command.CustomerId, Arg.Any<CancellationToken>())
            .Returns(customer);

        _dispatchRepositoryMock.GetByIdAsync(Command.DispatchId, Arg.Any<CancellationToken>())
            .Returns(dispatch);

        _orderRepositoryMock.IsOverlappingAsync(Arg.Any<Dispatch>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Criar uma classe fake do ShippingPriceService
        var fakeShippingService = new TestShippingPriceService();

        // Reconstruir o handler com o serviço fake
        var handlerWithFakeService = new ProcessOrderCommandHandler(
            _customerRepositoryMock,
            _dispatchRepositoryMock,
            _orderRepositoryMock,
            _unitOfWorkMock,
            fakeShippingService,
            _dateTimeProviderMock);

        // Act
        var result = await handlerWithFakeService.Handle(Command, default);

        // Debug: Exibir o resultado
        Debug.WriteLine($"Debug - Resultado: IsSuccess={result.IsSuccess}, Error={result.Error}");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    // Classe auxiliar para testes
    public class TestShippingPriceService : ShippingPriceService
    {
        // Sobrescrever o método CalcularFrete para retornar um valor fixo sem fazer chamadas reais
        public override Task<ShippingPriceDetails> CalcularFrete(Dispatch dispatch, ZipCode zipCode)
        {
            return Task.FromResult(new ShippingPriceDetails(1, "SEDEX", "25.00"));
        }
    }
}
