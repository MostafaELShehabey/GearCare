using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class inittt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_photos_userphotoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_userphotoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "WinchOrders");

            migrationBuilder.DropColumn(
                name: "userphotoId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "WinchOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "userphotoId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_userphotoId",
                table: "AspNetUsers",
                column: "userphotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_photos_userphotoId",
                table: "AspNetUsers",
                column: "userphotoId",
                principalTable: "photos",
                principalColumn: "Id");
        }
    }
}
