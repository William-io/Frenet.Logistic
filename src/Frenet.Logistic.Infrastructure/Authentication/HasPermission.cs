using Microsoft.AspNetCore.Authorization;

namespace Frenet.Logistic.Infrastructure.Authentication;

public sealed class HasPermission : AuthorizeAttribute
{
    public HasPermission(Permission permission) : base(policy: permission.ToString())
    {
    }
}
