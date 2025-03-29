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
        
        return new HashSet<string> { "ReadMember", "UpdateMember" };

    }
}
