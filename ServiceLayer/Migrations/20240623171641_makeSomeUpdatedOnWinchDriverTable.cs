using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class makeSomeUpdatedOnWinchDriverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrders_AspNetUsers_wichClient",
                table: "WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrders_WinchDrivers_DriverId",
                table: "WinchOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "WinchOrders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "WinchDrivers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "WinchDrivers");

            migrationBuilder.RenameColumn(
                name: "wichClient",
                table: "WinchOrders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrders_wichClient",
                table: "WinchOrders",
                newName: "IX_WinchOrders_ClientId");

            migrationBuilder.RenameColumn(
                name: "DriverLicence",
                table: "WinchDrivers",
                newName: "DriveringLicence");

            migrationBuilder.AddColumn<string>(
                name: "WinchDriverId",
                table: "WinchOrders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WinchOrders_WinchDriverId",
                table: "WinchOrders",
                column: "WinchDriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrders_AspNetUsers_ClientId",
                table: "WinchOrders",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrders_AspNetUsers_DriverId",
                table: "WinchOrders",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrders_WinchDrivers_WinchDriverId",
                table: "WinchOrders",
                column: "WinchDriverId",
                principalTable: "WinchDrivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrders_AspNetUsers_ClientId",
                table: "WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrders_AspNetUsers_DriverId",
                table: "WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrders_WinchDrivers_WinchDriverId",
                table: "WinchOrders");

            migrationBuilder.DropIndex(
                name: "IX_WinchOrders_WinchDriverId",
                table: "WinchOrders");

            migrationBuilder.DropColumn(
                name: "WinchDriverId",
                table: "WinchOrders");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "WinchOrders",
                newName: "wichClient");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrders_ClientId",
                table: "WinchOrders",
                newName: "IX_WinchOrders_wichClient");

            migrationBuilder.RenameColumn(
                name: "DriveringLicence",
                table: "WinchDrivers",
                newName: "DriverLicence");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "WinchOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WinchDrivers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber",
                table: "WinchDrivers",
                type: "int",
                maxLength: 11,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrders_AspNetUsers_wichClient",
                table: "WinchOrders",
                column: "wichClient",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrders_WinchDrivers_DriverId",
                table: "WinchOrders",
                column: "DriverId",
                principalTable: "WinchDrivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
