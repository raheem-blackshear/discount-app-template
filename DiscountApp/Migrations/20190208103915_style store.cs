using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountApp.Migrations
{
    public partial class stylestore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreStyle_Stores_StoreId",
                table: "StoreStyle");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreStyle_Styles_StyleId",
                table: "StoreStyle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreStyle",
                table: "StoreStyle");

            migrationBuilder.RenameTable(
                name: "StoreStyle",
                newName: "StoreStyles");

            migrationBuilder.RenameIndex(
                name: "IX_StoreStyle_StoreId",
                table: "StoreStyles",
                newName: "IX_StoreStyles_StoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreStyles",
                table: "StoreStyles",
                columns: new[] { "StyleId", "StoreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StoreStyles_Stores_StoreId",
                table: "StoreStyles",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreStyles_Styles_StyleId",
                table: "StoreStyles",
                column: "StyleId",
                principalTable: "Styles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreStyles_Stores_StoreId",
                table: "StoreStyles");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreStyles_Styles_StyleId",
                table: "StoreStyles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreStyles",
                table: "StoreStyles");

            migrationBuilder.RenameTable(
                name: "StoreStyles",
                newName: "StoreStyle");

            migrationBuilder.RenameIndex(
                name: "IX_StoreStyles_StoreId",
                table: "StoreStyle",
                newName: "IX_StoreStyle_StoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreStyle",
                table: "StoreStyle",
                columns: new[] { "StyleId", "StoreId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StoreStyle_Stores_StoreId",
                table: "StoreStyle",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreStyle_Styles_StyleId",
                table: "StoreStyle",
                column: "StyleId",
                principalTable: "Styles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
