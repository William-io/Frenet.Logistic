using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using static Frenet.Logistic.Domain.UnitTests.Orders.ShippingPriceServiceSettings;

namespace Frenet.Logistic.Domain.UnitTests.Orders;

public class ShippingPriceServiceTests
{
    [Fact]
    public async Task Calcular_Frete_Utilizando_API_Melhor_Envio()
    {
        // Arrange
        var dispatchParams = new Dispatch(
            Guid.NewGuid(),
            new PackageParams(4.723, 20, 79, 1)
        );
        var dispatch = SeedDispatchs.CreateDispatch(dispatchParams);


        var zipCode = new ZipCode("01002001", "60421010");
        var localTo_LocalFrom = SeedDispatchs.CreateZipCode(zipCode);

        // Cria um logger mock
        var loggerMock = new Mock<ILogger<ShippingPriceService>>();


        // Cria uma configuração mock
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["ShippingService:BaseUrl"]).Returns(_Settings.BaseUrl);
        configurationMock.Setup(x => x["ShippingService:ApiKey"]).Returns(_Settings.ApiKey);
        configurationMock.Setup(x => x["ShippingService:UserAgent"]).Returns(_Settings.UserAgent);

        var acessService = new ShippingPriceService(configurationMock.Object, loggerMock.Object);

        //Act
        var details = await acessService.CalcularFrete(dispatch, localTo_LocalFrom);

        //Verificando se o valor retornado não é nulo 
        details.Should().NotBeNull();
        //Verificando se o valor retornado é do tipo ShippingPriceDetails
        details.Should().BeOfType<ShippingPriceDetails>();

        Debug.WriteLine($"Resultado: Id={details.Id}, Nome={details.Name}, Preço={details.Price}");

    }

}
