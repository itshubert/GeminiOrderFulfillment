using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiOrderFulfillment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fullment_OrderId_Indexed_Unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Fulfillments_OrderId",
                table: "Fulfillments",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fulfillments_OrderId",
                table: "Fulfillments");
        }
    }
}
