using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateRepaireoprdertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "RepareOrders");

            migrationBuilder.AddColumn<string>(
                name: "cartype",
                table: "RepareOrders",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "RepareOrders",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cartype",
                table: "RepareOrders");

            migrationBuilder.DropColumn(
                name: "location",
                table: "RepareOrders");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "RepareOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
