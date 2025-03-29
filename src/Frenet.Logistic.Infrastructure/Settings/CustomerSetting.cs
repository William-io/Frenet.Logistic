
using Frenet.Logistic.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frenet.Logistic.Infrastructure.Settings;

internal sealed class CustomerSetting : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(customer => customer.Id);


        builder.Property(customer => customer.FirstName)
            .HasMaxLength(200)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value))
            .IsRequired();

        builder.Property(customer => customer.LastName)
            .HasMaxLength(200)
            .HasConversion(lastName => lastName.Value, value => new LastName(value))
            .IsRequired();

        builder
            .Property(customer => customer.Email)
            .HasConversion(email => email.Value, email => Domain.Customers.Email.Create(email).Value);

        builder.HasIndex(customer => customer.Email)
            .IsUnique();

        builder.Property(customer => customer.Phone)
            .HasMaxLength(15)
            .HasConversion(firstName => firstName.Value, value => new Phone(value))
            .IsRequired();

        builder.OwnsOne(customer => customer.Address);    
          

    }
}
