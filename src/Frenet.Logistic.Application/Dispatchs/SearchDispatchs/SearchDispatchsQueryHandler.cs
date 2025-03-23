

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
        if (request.Id != Guid.Empty)
        {
            return new List<DispatchResponse>();
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        
        const string sql = """
                   SELECT 
                        d.id AS Id,
                        d.Weight AS Weight,
                        d.Height AS Height,
                        d.Width AS Width,
                        d.Length AS Length
                   FROM dispatchs d
                   """;
        
        var dispatchs = await connection
            .QueryAsync<DispatchResponse, PackageParamsResponse, DispatchResponse>(
                sql,
                (dispatch, packageParams) =>
                {
                    dispatch.PackageParams = packageParams;
                    return dispatch;
                });

        return dispatchs.ToList();

    }
}