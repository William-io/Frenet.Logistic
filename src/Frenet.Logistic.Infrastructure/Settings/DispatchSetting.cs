using Frenet.Logistic.Domain.Dispatchs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frenet.Logistic.Infrastructure.Settings;

internal sealed class DispatchSetting : IEntityTypeConfiguration<Dispatch>
{
    public void Configure(EntityTypeBuilder<Dispatch> builder)
    {
        builder.ToTable("Dispatchs");

        builder.HasKey(dispatch => dispatch.Id);
        builder.OwnsOne(dispatch => dispatch.PackageParams);
    }
}
