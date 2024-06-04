using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class makewinchrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "photo",
                table: "Winchs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Licence",
                table: "Winchs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "WinchDrivers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WinchDrivers_DriverId",
                table: "WinchDrivers",
                column: "DriverId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WinchDrivers_AspNetUsers_DriverId",
                table: "WinchDrivers",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WinchDrivers_AspNetUsers_DriverId",
                table: "WinchDrivers");

            migrationBuilder.DropIndex(
                name: "IX_WinchDrivers_DriverId",
                table: "WinchDrivers");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "WinchDrivers");

            migrationBuilder.AlterColumn<string>(
                name: "photo",
                table: "Winchs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Licence",
                table: "Winchs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
