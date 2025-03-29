using Frenet.Logistic.Domain.Dispatchs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Frenet.Logistic.Domain.Orders;

public class ShippingPriceService
{
    private readonly string _baseUrl;
    private readonly string _apiKey;
    private readonly string _userAgent;

    public ShippingPriceService(IConfiguration configuration)
    {

        _baseUrl = configuration["ShippingService:BaseUrl"];
        _apiKey = configuration["ShippingService:ApiKey"];
        _userAgent = configuration["ShippingService:UserAgent"];
    }

    public async Task<ShippingPriceDetails> CalcularFrete(Dispatch dispatch, ZipCode zipCode)
    {
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
        //Erro no response é preciso passar os dados de conexao ao serviço. 
        request.AddJsonBody(pacote);

        var response = await client.PostAsync(request);
        
        //Obtencao
        if (response.IsSuccessful && response.Content != null)
        {
            try
            {
                var shippingDetails = JsonConvert.DeserializeObject<List<ShippingPriceDetails>>(response.Content);
                var company = shippingDetails?.FirstOrDefault(pd => pd.Id == 3);
                
                if (company != null)
                {
                    //teste
                    Console.WriteLine("Company Name: {0} - Price: {1}", company.Name, company.Price);
                    return new ShippingPriceDetails(company.Id, company.Name, company.Price);
                }
                else
                {
                    throw new ApplicationException("Não foi possível encontrar os detalhes de envio para a companhia especificada.");
                }
    
            }
            catch (JsonException e)
            {
                throw new ApplicationException($"An error occurred while processing the shipping price details. JSON Deserialization Error: {e.Message}");
            }
        }

        Console.WriteLine("{0}", response);
        throw new ApplicationException("A resposta não foi bem-sucedida ou o conteúdo está vazio.");
    }
}