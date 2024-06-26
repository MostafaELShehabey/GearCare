using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class anotherUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_WinchOrders",
                table: "WinchOrders");

            migrationBuilder.DropIndex(
                name: "IX_WinchOrders_DriverId",
                table: "WinchOrders");

            migrationBuilder.RenameTable(
                name: "WinchOrders",
                newName: "WinchOrder");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrders_WinchDriverId",
                table: "WinchOrder",
                newName: "IX_WinchOrder_WinchDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrders_ClientId",
                table: "WinchOrder",
                newName: "IX_WinchOrder_ClientId");

            migrationBuilder.AlterColumn<string>(
                name: "DriverId",
                table: "WinchOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WinchOrder",
                table: "WinchOrder",
                column: "OrderId");

            migrationBuilder.CreateTable(
                name: "applicationUser_WinchOrders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WinchOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationUser_WinchOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_applicationUser_WinchOrders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_applicationUser_WinchOrders_WinchOrder_WinchOrderId",
                        column: x => x.WinchOrderId,
                        principalTable: "WinchOrder",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_applicationUser_WinchOrders_ApplicationUserId",
                table: "applicationUser_WinchOrders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_applicationUser_WinchOrders_WinchOrderId",
                table: "applicationUser_WinchOrders",
                column: "WinchOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrder_AspNetUsers_ClientId",
                table: "WinchOrder",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrder_WinchDrivers_WinchDriverId",
                table: "WinchOrder",
                column: "WinchDriverId",
                principalTable: "WinchDrivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrder_AspNetUsers_ClientId",
                table: "WinchOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrder_WinchDrivers_WinchDriverId",
                table: "WinchOrder");

            migrationBuilder.DropTable(
                name: "applicationUser_WinchOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WinchOrder",
                table: "WinchOrder");

            migrationBuilder.RenameTable(
                name: "WinchOrder",
                newName: "WinchOrders");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrder_WinchDriverId",
                table: "WinchOrders",
                newName: "IX_WinchOrders_WinchDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrder_ClientId",
                table: "WinchOrders",
                newName: "IX_WinchOrders_ClientId");

            migrationBuilder.AlterColumn<string>(
                name: "DriverId",
                table: "WinchOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WinchOrders",
                table: "WinchOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WinchOrders_DriverId",
                table: "WinchOrders",
                column: "DriverId");

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
    }
}
