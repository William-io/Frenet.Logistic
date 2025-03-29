﻿// <auto-generated />
using System;
using Frenet.Logistic.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Frenet.Logistic.Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CustomerRole", b =>
                {
                    b.Property<Guid>("CustomersId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("customers_id");

                    b.Property<int>("RolesId")
                        .HasColumnType("int")
                        .HasColumnName("roles_id");

                    b.HasKey("CustomersId", "RolesId")
                        .HasName("pk_customer_role");

                    b.HasIndex("RolesId")
                        .HasDatabaseName("ix_customer_role_roles_id");

                    b.ToTable("customer_role", (string)null);
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Customers.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("last_name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("phone");

                    b.HasKey("Id")
                        .HasName("pk_customers");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_customers_email");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Customers.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_permissions");

                    b.ToTable("Permissions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "ReadMember"
                        },
                        new
                        {
                            Id = 2,
                            Name = "UpdateMember"
                        });
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Customers.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Registered"
                        });
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Customers.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int")
                        .HasColumnName("permission_id");

                    b.HasKey("RoleId", "PermissionId")
                        .HasName("pk_role_permission");

                    b.HasIndex("PermissionId")
                        .HasDatabaseName("ix_role_permission_permission_id");

                    b.ToTable("role_permission", (string)null);

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermissionId = 1
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 2
                        });
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Dispatchs.Dispatch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTime?>("LastDispatchOnUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("last_dispatch_on_utc");

                    b.HasKey("Id")
                        .HasName("pk_dispatchs");

                    b.ToTable("Dispatchs", (string)null);
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTime>("CancelledOnUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("cancelled_on_utc");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on_utc");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("customer_id");

                    b.Property<DateTime>("DeliveredOnUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("delivered_on_utc");

                    b.Property<Guid>("DispatchId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("dispatch_id");

                    b.Property<DateTime>("ProcessingOnUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("processing_on_utc");

                    b.Property<DateTime>("ShippedOnUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("shipped_on_utc");

                    b.Property<string>("ShippingName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("shipping_name");

                    b.Property<string>("ShippingPrice")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("shipping_price");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("ix_orders_customer_id");

                    b.HasIndex("DispatchId")
                        .HasDatabaseName("ix_orders_dispatch_id");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("CustomerRole", b =>
                {
                    b.HasOne("Frenet.Logistic.Domain.Customers.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_customer_role_customer_customers_id");

                    b.HasOne("Frenet.Logistic.Domain.Customers.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_customer_role_role_roles_id");
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Customers.Customer", b =>
                {
                    b.OwnsOne("Frenet.Logistic.Domain.Customers.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("id");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("address_city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("address_country");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("address_state");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("address_street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("address_zip_code");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId")
                                .HasConstraintName("fk_customers_customers_id");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Customers.RolePermission", b =>
                {
                    b.HasOne("Frenet.Logistic.Domain.Customers.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_permission_permissions_permission_id");

                    b.HasOne("Frenet.Logistic.Domain.Customers.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_permission_roles_role_id");
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Dispatchs.Dispatch", b =>
                {
                    b.OwnsOne("Frenet.Logistic.Domain.Dispatchs.PackageParams", "Package", b1 =>
                        {
                            b1.Property<Guid>("DispatchId")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("id");

                            b1.Property<int>("Height")
                                .HasColumnType("int")
                                .HasColumnName("package_height");

                            b1.Property<int>("Length")
                                .HasColumnType("int")
                                .HasColumnName("package_length");

                            b1.Property<double>("Weight")
                                .HasColumnType("float")
                                .HasColumnName("package_weight");

                            b1.Property<int>("Width")
                                .HasColumnType("int")
                                .HasColumnName("package_width");

                            b1.HasKey("DispatchId");

                            b1.ToTable("Dispatchs");

                            b1.WithOwner()
                                .HasForeignKey("DispatchId")
                                .HasConstraintName("fk_dispatchs_dispatchs_id");
                        });

                    b.Navigation("Package")
                        .IsRequired();
                });

            modelBuilder.Entity("Frenet.Logistic.Domain.Orders.Order", b =>
                {
                    b.HasOne("Frenet.Logistic.Domain.Customers.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_customers_customer_id");

                    b.HasOne("Frenet.Logistic.Domain.Dispatchs.Dispatch", null)
                        .WithMany()
                        .HasForeignKey("DispatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_dispatchs_dispatch_id");

                    b.OwnsOne("Frenet.Logistic.Domain.Orders.ZipCode", "ZipCode", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("id");

                            b1.Property<string>("CodeFrom")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("zip_code_from");

                            b1.Property<string>("CodeTo")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("zip_code_to");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId")
                                .HasConstraintName("fk_orders_orders_id");
                        });

                    b.Navigation("ZipCode")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
