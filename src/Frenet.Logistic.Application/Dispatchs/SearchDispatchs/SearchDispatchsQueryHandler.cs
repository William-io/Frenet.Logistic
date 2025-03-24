

using Dapper;
using Frenet.Logistic.Application.Abstractions.DataFactory;
using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Application.Dispatchs.SearchDispatchs;

internal class SearchDispatchsQueryHandler 
    : IQueryHandler<SearchDispatchsQuery, IReadOnlyList<DispatchResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchDispatchsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<DispatchResponse>>> Handle(SearchDispatchsQuery request, CancellationToken cancellationToken)
    {

        using var connection = _sqlConnectionFactory.CreateConnection();
        
        const string sql = """
                   SELECT 
                        d.id AS Id,
                        d.package_weight AS Weight,
                        d.package_height AS Height,
                        d.package_width AS Width,
                        d.package_length AS Length
                   FROM dispatchs d
                   WHERE d.id = @Id
                   """;

        var dispatchs = await connection.QueryAsync<DispatchResponse, PackageParamsResponse, DispatchResponse>(
            sql,
            (dispatch, packageParams) =>
            {
                dispatch.Package = packageParams;
                return dispatch;
            },
            new { request.Id },
            splitOn: "Weight");

        return dispatchs.ToList();  

    }
}