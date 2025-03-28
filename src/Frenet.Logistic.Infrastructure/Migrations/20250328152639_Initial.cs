using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frenet.Logistic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    address_country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address_state = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address_zip_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address_street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Dispatchs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    last_dispatch_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    package_weight = table.Column<double>(type: "float", nullable: false),
                    package_height = table.Column<int>(type: "int", nullable: false),
                    package_width = table.Column<int>(type: "int", nullable: false),
                    package_length = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dispatchs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dispatch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    zip_code_from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zip_code_to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    processing_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shipped_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    delivered_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cancelled_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    shipping_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    shipping_price = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_dispatchs_dispatch_id",
                        column: x => x.dispatch_id,
                        principalTable: "Dispatchs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_customers_email",
                table: "Customers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_customer_id",
                table: "Order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_dispatch_id",
                table: "Order",
                column: "dispatch_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Dispatchs");
        }
    }
}
