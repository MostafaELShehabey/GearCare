using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class addkeytoWinchOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ShoppingCarts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WinchOrders",
                newName: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "WinchOrders",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
