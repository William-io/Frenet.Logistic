using Frenet.Logistic.Domain.Dispatchs;
using Newtonsoft.Json;
using RestSharp;

namespace Frenet.Logistic.Domain.Orders;

public class ShippingPriceService
{
    private static readonly string BaseUrl = "https://melhorenvio.com.br/api/v2/me/shipment/calculate";
    private static readonly string ApiKey = "your-api-key";
    private static readonly string UserAgent = "Aplicacao/1.0";

    public async Task<ShippingPriceDetails> CalcularFrete(Dispatch dispatch, ZipCode zipCode)
    {
        var options = new RestClientOptions(BaseUrl);
        var client = new RestClient(options);

        var request = new RestRequest
        {
            Method = Method.Post
        };

        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {ApiKey}");
        request.AddHeader("User-Agent", UserAgent);


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
                height = dispatch.PackageParams.Height,
                width = dispatch.PackageParams.Width,
                length = dispatch.PackageParams.Length,
                weight = dispatch.PackageParams.Weight
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
                var company = shippingDetails?.FirstOrDefault(pd => pd.Id == 2);
                
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