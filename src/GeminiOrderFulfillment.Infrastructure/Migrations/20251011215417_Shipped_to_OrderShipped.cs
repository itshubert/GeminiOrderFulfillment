using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiOrderFulfillment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Shipped_to_OrderShipped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Fulfillment_Status",
                table: "Fulfillments");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Fulfillment_Status",
                table: "Fulfillments",
                sql: "\"Status\" IN ('AWAITING_FULFILLMENT', 'TASK_CREATED', 'PICKING_IN_PROGRESS', 'PACKED', 'LABEL_GENERATED', 'ORDER_SHIPPED', 'IN_TRANSIT', 'DELIVERED')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Fulfillment_Status",
                table: "Fulfillments");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Fulfillment_Status",
                table: "Fulfillments",
                sql: "\"Status\" IN ('AWAITING_FULFILLMENT', 'TASK_CREATED', 'PICKING_IN_PROGRESS', 'PACKED', 'LABEL_GENERATED', 'SHIPPED', 'IN_TRANSIT', 'DELIVERED')");
        }
    }
}
