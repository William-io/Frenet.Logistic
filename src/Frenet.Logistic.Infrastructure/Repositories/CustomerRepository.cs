using Frenet.Logistic.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Frenet.Logistic.Infrastructure.Repositories;

internal sealed class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly Context _context;

    public CustomerRepository(Context context) : base(context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByEmailAsync(Domain.Customers.Email email, CancellationToken cancellationToken = default) =>
        await _context
            .Set<Customer>()
            .FirstOrDefaultAsync(member => member.Email == email, cancellationToken);
   
}
