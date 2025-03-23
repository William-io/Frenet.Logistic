

using Dapper;
using Frenet.Logistic.Application.Abstractions.DataFactory;
using Frenet.Logistic.Application.Abstractions.Messaging;
using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Application.Orders.GetOrder;

internal sealed class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetOrderQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<OrderResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id as Id,
                order_id AS OrderId,
                customer_id AS CustomerId,
                status AS Status,
                created_on_utc AS CreatedOnUtc,
                shipping_name AS ShippingName,
                shipping_price AS ShippingPrice
            FROM orders
            WHERE id = @OrderId
            """;

        var order = await connection.QueryFirstOrDefaultAsync<OrderResponse>(
            sql,
            new
            {
                request.OrderId
            });



        return order;
    }
}
