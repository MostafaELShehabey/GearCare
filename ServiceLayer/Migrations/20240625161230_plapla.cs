using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class plapla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applicationUser_WinchOrders_AspNetUsers_ApplicationUserId",
                table: "applicationUser_WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_applicationUser_WinchOrders_WinchOrder_WinchOrderId",
                table: "applicationUser_WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_photos_Products_productID",
                table: "photos");

            migrationBuilder.DropForeignKey(
                name: "FK_product_Shoppingcarts_Products_ProductId",
                table: "product_Shoppingcarts");

            migrationBuilder.DropForeignKey(
                name: "FK_product_Shoppingcarts_ShoppingCarts_ShoppingcartId",
                table: "product_Shoppingcarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategorysId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_discounts_Discountid",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrder_AspNetUsers_ClientId",
                table: "WinchOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrder_WinchDrivers_WinchDriverId",
                table: "WinchOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_Shoppingcarts",
                table: "product_Shoppingcarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_photos",
                table: "photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_discounts",
                table: "discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_applicationUser_WinchOrders",
                table: "applicationUser_WinchOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WinchOrder",
                table: "WinchOrder");

            migrationBuilder.RenameTable(
                name: "product_Shoppingcarts",
                newName: "Product_Shoppingcarts");

            migrationBuilder.RenameTable(
                name: "photos",
                newName: "Photos");

            migrationBuilder.RenameTable(
                name: "discounts",
                newName: "Discounts");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameTable(
                name: "applicationUser_WinchOrders",
                newName: "ApplicationUser_WinchOrders");

            migrationBuilder.RenameTable(
                name: "WinchOrder",
                newName: "WinchOrders");

            migrationBuilder.RenameIndex(
                name: "IX_product_Shoppingcarts_ShoppingcartId",
                table: "Product_Shoppingcarts",
                newName: "IX_Product_Shoppingcarts_ShoppingcartId");

            migrationBuilder.RenameIndex(
                name: "IX_product_Shoppingcarts_ProductId",
                table: "Product_Shoppingcarts",
                newName: "IX_Product_Shoppingcarts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_photos_productID",
                table: "Photos",
                newName: "IX_Photos_productID");

            migrationBuilder.RenameIndex(
                name: "IX_applicationUser_WinchOrders_WinchOrderId",
                table: "ApplicationUser_WinchOrders",
                newName: "IX_ApplicationUser_WinchOrders_WinchOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_applicationUser_WinchOrders_ApplicationUserId",
                table: "ApplicationUser_WinchOrders",
                newName: "IX_ApplicationUser_WinchOrders_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrder_WinchDriverId",
                table: "WinchOrders",
                newName: "IX_WinchOrders_WinchDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrder_ClientId",
                table: "WinchOrders",
                newName: "IX_WinchOrders_ClientId");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "WinchOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientPhoto",
                table: "WinchOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "WinchOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product_Shoppingcarts",
                table: "Product_Shoppingcarts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser_WinchOrders",
                table: "ApplicationUser_WinchOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WinchOrders",
                table: "WinchOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_WinchOrders_AspNetUsers_ApplicationUserId",
                table: "ApplicationUser_WinchOrders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_WinchOrders_WinchOrders_WinchOrderId",
                table: "ApplicationUser_WinchOrders",
                column: "WinchOrderId",
                principalTable: "WinchOrders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Products_productID",
                table: "Photos",
                column: "productID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Shoppingcarts_Products_ProductId",
                table: "Product_Shoppingcarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Shoppingcarts_ShoppingCarts_ShoppingcartId",
                table: "Product_Shoppingcarts",
                column: "ShoppingcartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Discounts_Discountid",
                table: "Products",
                column: "Discountid",
                principalTable: "Discounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_categories_CategorysId",
                table: "Products",
                column: "CategorysId",
                principalTable: "categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WinchOrders_AspNetUsers_ClientId",
                table: "WinchOrders",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_ApplicationUser_WinchOrders_AspNetUsers_ApplicationUserId",
                table: "ApplicationUser_WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_WinchOrders_WinchOrders_WinchOrderId",
                table: "ApplicationUser_WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Products_productID",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Shoppingcarts_Products_ProductId",
                table: "Product_Shoppingcarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Shoppingcarts_ShoppingCarts_ShoppingcartId",
                table: "Product_Shoppingcarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Discounts_Discountid",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_categories_CategorysId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrders_AspNetUsers_ClientId",
                table: "WinchOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WinchOrders_WinchDrivers_WinchDriverId",
                table: "WinchOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product_Shoppingcarts",
                table: "Product_Shoppingcarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discounts",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser_WinchOrders",
                table: "ApplicationUser_WinchOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WinchOrders",
                table: "WinchOrders");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "WinchOrders");

            migrationBuilder.DropColumn(
                name: "ClientPhoto",
                table: "WinchOrders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "WinchOrders");

            migrationBuilder.RenameTable(
                name: "Product_Shoppingcarts",
                newName: "product_Shoppingcarts");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "photos");

            migrationBuilder.RenameTable(
                name: "Discounts",
                newName: "discounts");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "ApplicationUser_WinchOrders",
                newName: "applicationUser_WinchOrders");

            migrationBuilder.RenameTable(
                name: "WinchOrders",
                newName: "WinchOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Shoppingcarts_ShoppingcartId",
                table: "product_Shoppingcarts",
                newName: "IX_product_Shoppingcarts_ShoppingcartId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Shoppingcarts_ProductId",
                table: "product_Shoppingcarts",
                newName: "IX_product_Shoppingcarts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_productID",
                table: "photos",
                newName: "IX_photos_productID");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_WinchOrders_WinchOrderId",
                table: "applicationUser_WinchOrders",
                newName: "IX_applicationUser_WinchOrders_WinchOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_WinchOrders_ApplicationUserId",
                table: "applicationUser_WinchOrders",
                newName: "IX_applicationUser_WinchOrders_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrders_WinchDriverId",
                table: "WinchOrder",
                newName: "IX_WinchOrder_WinchDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_WinchOrders_ClientId",
                table: "WinchOrder",
                newName: "IX_WinchOrder_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_Shoppingcarts",
                table: "product_Shoppingcarts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_photos",
                table: "photos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_discounts",
                table: "discounts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_applicationUser_WinchOrders",
                table: "applicationUser_WinchOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WinchOrder",
                table: "WinchOrder",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_applicationUser_WinchOrders_AspNetUsers_ApplicationUserId",
                table: "applicationUser_WinchOrders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_applicationUser_WinchOrders_WinchOrder_WinchOrderId",
                table: "applicationUser_WinchOrders",
                column: "WinchOrderId",
                principalTable: "WinchOrder",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_photos_Products_productID",
                table: "photos",
                column: "productID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_Shoppingcarts_Products_ProductId",
                table: "product_Shoppingcarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_Shoppingcarts_ShoppingCarts_ShoppingcartId",
                table: "product_Shoppingcarts",
                column: "ShoppingcartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategorysId",
                table: "Products",
                column: "CategorysId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_discounts_Discountid",
                table: "Products",
                column: "Discountid",
                principalTable: "discounts",
                principalColumn: "id");

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
    }
}
