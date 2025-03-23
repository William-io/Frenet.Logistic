namespace Frenet.Logistic.Application.Dispatchs.SearchDispatchs;

public sealed class DispatchResponse
{
    public Guid Id { get; init; } 
    public PackageParamsResponse PackageParams { get; set; }
}