using Frenet.Logistic.Domain.Shared;

namespace Frenet.Logistic.Domain.Customers;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Registered = new(1, "Registered");

    public Role(int id, string name)
       : base(id, name)
    {
    }

    public ICollection<Customer> Customers { get; set; }

    public ICollection<Permission> Permissions { get; set; } 
}
