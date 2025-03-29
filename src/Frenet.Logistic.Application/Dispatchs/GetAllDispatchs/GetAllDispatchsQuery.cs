using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Dispatchs.GetAllDispatchs;

public sealed record GetAllDispatchsQuery() : IQuery<IReadOnlyList<GetAllDispatchsResponse>>;

