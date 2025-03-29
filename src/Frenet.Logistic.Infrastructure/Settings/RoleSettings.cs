using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frenet.Logistic.Infrastructure.Settings;

internal sealed class RoleSettings : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(Tables.Roles);

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany(x => x.Customers)
           .WithMany(x => x.Roles);

        builder.HasData(Role.GetValues());
    }
}
