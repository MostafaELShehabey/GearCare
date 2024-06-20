using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class repareorderupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "RepareOrders");

            migrationBuilder.AddColumn<string>(
                name: "ServiceProviderId",
                table: "RepareOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cartype",
                table: "RepareOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "RepareOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "price",
                table: "Products",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "newPrice",
                table: "Products",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceProviderId",
                table: "RepareOrders");

            migrationBuilder.DropColumn(
                name: "cartype",
                table: "RepareOrders");

            migrationBuilder.DropColumn(
                name: "location",
                table: "RepareOrders");

            migrationBuilder.DropColumn(
                name: "newPrice",
                table: "Products");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "RepareOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "price",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
