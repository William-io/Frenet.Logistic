using Frenet.Logistic.Domain.Dispatchs;

namespace Frenet.Logistic.Infrastructure.Repositories;

internal sealed class DispatchRepository : Repository<Dispatch>, IDispatchRepository
{
    public DispatchRepository(Context context) : base(context)
    {
    }
}
