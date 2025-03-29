using Frenet.Logistic.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Frenet.Logistic.Domain.Enums.Permission;

namespace Frenet.Logistic.Infrastructure.Settings;

internal class RolePermissionSettings : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasData(
            Create(Role.Registered, Permission.ReadMember),
            Create(Role.Registered, Permission.UpdateMember));
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission
        };
    }
}
