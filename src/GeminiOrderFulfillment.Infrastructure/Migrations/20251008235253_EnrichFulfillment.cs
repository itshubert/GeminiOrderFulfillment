using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiOrderFulfillment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnrichFulfillment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Fulfillments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Fulfillments",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Fulfillments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Fulfillments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Fulfillments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Fulfillments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Fulfillments",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Fulfillments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Fulfillments");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Fulfillments");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Fulfillments");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Fulfillments");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Fulfillments");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Fulfillments");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Fulfillments");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Fulfillments");
        }
    }
}
