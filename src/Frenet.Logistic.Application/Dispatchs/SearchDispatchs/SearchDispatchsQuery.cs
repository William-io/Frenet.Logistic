using Frenet.Logistic.Application.Abstractions.Messaging;

namespace Frenet.Logistic.Application.Dispatchs.SearchDispatchs;

public sealed record SearchDispatchsQuery(Guid Id) : IQuery<IReadOnlyList<DispatchResponse>>;