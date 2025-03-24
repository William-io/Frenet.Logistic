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

        for (int i = 0; i < 10; i++)
        {
            dispatch.Add(new
            {
                Id = Guid.NewGuid(),
                Weight = faker.Random.Int(1, 100), // Peso entre 0.1 e 100.0
                Height = faker.Random.Int(1, 100), // Altura entre 1 e 200 cm
                Width = faker.Random.Int(1, 100), // Largura entre 1 e 200 cm
                Length = faker.Random.Int(1, 100), // Comprimento entre 1 e 200 cm
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
