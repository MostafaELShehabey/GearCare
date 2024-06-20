using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class makeOrderUserRelationManytoMay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepareOrders_AspNetUsers_ClientId",
                table: "RepareOrders");

            migrationBuilder.CreateTable(
                name: "RepairOrder_ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    orderId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrder_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRepareOrder_ApplicationUser",
                columns: table => new
                {
                    RepairOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    applicationUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRepareOrder_ApplicationUser", x => new { x.RepairOrderId, x.applicationUsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRepareOrder_ApplicationUser_AspNetUsers_applicationUsersId",
                        column: x => x.applicationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRepareOrder_ApplicationUser_RepairOrder_ApplicationUsers_RepairOrderId",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder_ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRepareOrder_ApplicationUser_applicationUsersId",
                table: "ApplicationUserRepareOrder_ApplicationUser",
                column: "applicationUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepareOrders_RepairOrder_ApplicationUsers_ClientId",
                table: "RepareOrders",
                column: "ClientId",
                principalTable: "RepairOrder_ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepareOrders_RepairOrder_ApplicationUsers_ClientId",
                table: "RepareOrders");

            migrationBuilder.DropTable(
                name: "ApplicationUserRepareOrder_ApplicationUser");

            migrationBuilder.DropTable(
                name: "RepairOrder_ApplicationUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_RepareOrders_AspNetUsers_ClientId",
                table: "RepareOrders",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
