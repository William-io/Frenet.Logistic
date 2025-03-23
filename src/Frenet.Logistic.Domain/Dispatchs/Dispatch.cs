using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Dispatchs;

public class Dispatch : Entity
{
    public Dispatch(Guid id, Package packageParams) : base(id)
    {
        PackageParams = packageParams;
    }

    public DateTime? LastDispatchOnUtc { get; internal set; }
    public Package PackageParams { get; private set; }

}