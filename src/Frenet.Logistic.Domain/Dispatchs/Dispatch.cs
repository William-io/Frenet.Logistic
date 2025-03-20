using Frenet.Logistic.Domain.Abstractions;

namespace Frenet.Logistic.Domain.Dispatchs;

public class Dispatch : Entity
{
    public Dispatch(Guid id) : base(id)
    {
    }

    public DateTime? LastDispatchOnUtc { get; private set; }
    
    
}