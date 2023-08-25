using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreprops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "RefundState");

            migrationBuilder.AddColumn<string>(
                name: "Fees",
                table: "RefundState",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Payments",
                table: "RefundState",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fees",
                table: "RefundState");

            migrationBuilder.DropColumn(
                name: "Payments",
                table: "RefundState");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "RefundState",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
