namespace Frenet.Logistic.Application.Dispatchs.GetAllDispatchs;

public sealed record GetAllDispatchsResponse(
    Guid Id,
    double Weight,
    int Height,
    int Width,
    int Length);
