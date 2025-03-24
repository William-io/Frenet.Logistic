using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Dispatchs;

public class Dispatch : Entity
{
    public Dispatch(Guid id, PackageParams packageParams) : base(id)
    {
        Package = packageParams;
    }

    private Dispatch() { }

    public DateTime? LastDispatchOnUtc { get; internal set; }
    public PackageParams Package { get; private set; }

}