using Frenet.Logistic.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Frenet.Logistic.Infrastructure.Authentication;

public class PermissionService : IPermissionService
{
    private readonly Context _context;

    public PermissionService(Context context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetPermissionsAsync(Guid customerId)
    {
        IReadOnlyCollection<Role>[] roles = await _context.Set<Customer>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == customerId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}
