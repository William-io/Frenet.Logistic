using Bogus;
using Dapper;
using Frenet.Logistic.Application.Abstractions.DataFactory;
using System.Data;

namespace Frenet.Logistic.API.Extensions;

internal static class Seeding
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        //Bogus para fake data
        var faker = new Faker();

        List<object> dispatch = new();

        /*Será populado a tabela dispatch, para armazenar parametros de envio de pacotes para ser consumir com API externa [MelhorEnvio]   
         */
        for (int i = 0; i < 10; i++)
        {
            int height, width, length;
            do
            {
                height = faker.Random.Int(1, 105); // Altura entre 1 e 105 cm
                width = faker.Random.Int(1, 105); // Largura entre 1 e 105 cm
                length = faker.Random.Int(1, 105); // Comprimento entre 1 e 105 cm
            } while (height + width + length > 200); // Soma das dimensões até 200 cm

            dispatch.Add(new
            {
                Id = Guid.NewGuid(),
                Weight = faker.Random.Int(1, 30000) / 1000.0, // Peso entre 0.1 e 30.0 kg
                Height = height,
                Width = width,
                Length = length,
                LastDispatchOnUtc = faker.Date.Past(1), // Data de envio
            });
        }

        const string sql = """
        INSERT INTO Dispatchs
        (id, package_weight, package_height, package_width, package_length, last_dispatch_on_utc)
        VALUES (@Id, @Weight, @Height, @Width, @Length, @LastDispatchOnUtc);
        """;

        connection.Execute(sql, dispatch);
    }
}
