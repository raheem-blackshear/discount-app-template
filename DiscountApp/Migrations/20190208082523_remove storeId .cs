using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountApp.Migrations
{
    public partial class removestoreId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Styles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Styles",
                nullable: false,
                defaultValue: 0);
        }
    }
}
