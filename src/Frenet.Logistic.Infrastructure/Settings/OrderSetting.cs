using Frenet.Logistic.Domain.Customers;
using Frenet.Logistic.Domain.Dispatchs;
using Frenet.Logistic.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frenet.Logistic.Infrastructure.Settings;

internal sealed class OrderSetting : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("order");

        builder.HasKey(order => order.Id);

        builder.HasOne<Dispatch>()
           .WithMany()
           .HasForeignKey(booking => booking.DispatchId);

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(booking => booking.CustomerId);

        builder.OwnsOne(order => order.ZipCode, zipCode =>
        {
            zipCode.Property(zip => zip.CodeFrom).HasColumnName("zip_code_from");
            zipCode.Property(zip => zip.CodeTo).HasColumnName("zip_code_to");
        });

        builder.Property(order => order.ShippingName)
            .HasColumnName("shipping_name")
            .HasMaxLength(100);

        builder.Property(order => order.ShippingPrice)
            .HasColumnName("shipping_price")
            .HasMaxLength(100);

    }
}
