using Frenet.Logistic.Domain.Customers;

namespace Frenet.Logistic.Infrastructure.Repositories;

internal sealed class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(Context context) : base(context)
    {
    }
}
