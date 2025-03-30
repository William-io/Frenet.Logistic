using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Orders.GetAllOrder;

public sealed record GetAllOrderQuery() : IQuery<IReadOnlyList<GetAllOrderResponse>>;
