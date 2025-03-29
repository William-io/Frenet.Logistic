using Dapper;
using Frenet.Logistic.Application.Abstractions.DataFactory;
using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;
using System.Data;

namespace Frenet.Logistic.Application.Dispatchs.GetAllDispatchs;

public sealed class GetAllDispatchsQueryHandler : IQueryHandler<GetAllDispatchsQuery, IReadOnlyList<GetAllDispatchsResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllDispatchsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<GetAllDispatchsResponse>>> Handle(GetAllDispatchsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            const string sql = """
                SELECT
                    id AS Id,
                    package_weight AS Weight,
                    package_height AS Height,
                    package_width AS Width,
                    package_length AS Length
                FROM dispatchs
                """;

            var dispatchs = await connection.QueryAsync<GetAllDispatchsResponse>(sql);

            return Result.Success<IReadOnlyList<GetAllDispatchsResponse>>(dispatchs.ToList());
        }
        catch (Exception ex)
        {
            // Log the exception (you can replace this with your logging framework)
            Console.WriteLine($"Erro ao consultar despachos: {ex.Message}");
            return Result.Failure<IReadOnlyList<GetAllDispatchsResponse>>(
                new Error("Dispatch.QueryFailed", "Falha ao consultar dados dos despachos"));
        }
    }
}
