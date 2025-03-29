using Frenet.Logistic.Domain.Dispatchs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.Extensions.Logging;

namespace Frenet.Logistic.Domain.Orders;

public class ShippingPriceService
{
    private readonly string _baseUrl;
    private readonly string _apiKey;
    private readonly string _userAgent;

    private readonly ILogger<ShippingPriceService> _logger;

    public ShippingPriceService(IConfiguration configuration, ILogger<ShippingPriceService> logger)
    {

        _baseUrl = configuration["ShippingService:BaseUrl"];
        _apiKey = configuration["ShippingService:ApiKey"];
        _userAgent = configuration["ShippingService:UserAgent"];
        _logger = logger;
    }

    public async Task<ShippingPriceDetails> CalcularFrete(Dispatch dispatch, ZipCode zipCode)
    {
        _logger.LogInformation($"Calculando frete para o pacote {dispatch.Package} de {zipCode.CodeFrom} para {zipCode.CodeTo}");

        var options = new RestClientOptions(_baseUrl);
        var client = new RestClient(options);

        var request = new RestRequest
        {
            Method = Method.Post
        };

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {_apiKey}");
        request.AddHeader("User-Agent", _userAgent);


        var pacote = new
        {
            from = new
            {
                postal_code = zipCode.CodeFrom
            },
            to = new
            {
                postal_code = zipCode.CodeTo
            },
            package = new
            {
                height = dispatch.Package.Height,
                width = dispatch.Package.Width,
                length = dispatch.Package.Length,
                weight = dispatch.Package.Weight
            }
        };
        
        request.AddJsonBody(pacote);

        var response = await client.PostAsync(request);
        
        //Obtencao
        if (response.IsSuccessful && response.Content != null)
        {
            try
            {
                var shippingDetails = JsonConvert.DeserializeObject<List<ShippingPriceDetails>>(response.Content);
                var company = shippingDetails?.FirstOrDefault(pd => pd.Id == 1);
                
                if (company != null)
                {
                    //teste
                    _logger.LogInformation("Company Name: {CompanyName} - Price: {Price}", company.Name, company.Price);
                    return new ShippingPriceDetails(company.Id, company.Name, company.Price);
                }
                else
                {
                    _logger.LogWarning("Não foi possível encontrar os detalhes de envio para a companhia especificada.");
                    throw new ApplicationException("Não foi possível encontrar os detalhes de envio para a companhia especificada.");
                }
    
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Erro ao processar os detalhes do preço de envio.");
                throw new ApplicationException($"Ocorreu um erro ao processar os detalhes do preço de envio. JSON Deserialization Error: {e.Message}");
            }
        }

        _logger.LogError("A resposta não foi bem-sucedida ou o conteúdo está vazio. Response: {Response}", response);
        throw new ApplicationException("A resposta não foi bem-sucedida ou o conteúdo está vazio.");
    }
}