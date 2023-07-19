using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscountApp.Migrations
{
    public partial class Questions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "TBL_User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OnlineShopping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineShopping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreferredStyle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredStyle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Age = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionCategory",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategory", x => new { x.QuestionId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_QuestionCategory_AppProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AppProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionCategory_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOnlineShopping",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false),
                    OnlineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOnlineShopping", x => new { x.QuestionId, x.OnlineId });
                    table.ForeignKey(
                        name: "FK_QuestionOnlineShopping_OnlineShopping_OnlineId",
                        column: x => x.OnlineId,
                        principalTable: "OnlineShopping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionOnlineShopping_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionPreferredStyle",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false),
                    PreferredId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionPreferredStyle", x => new { x.QuestionId, x.PreferredId });
                    table.ForeignKey(
                        name: "FK_QuestionPreferredStyle_PreferredStyle_PreferredId",
                        column: x => x.PreferredId,
                        principalTable: "PreferredStyle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionPreferredStyle_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionProductType",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false),
                    ProductTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionProductType", x => new { x.QuestionId, x.ProductTypeId });
                    table.ForeignKey(
                        name: "FK_QuestionProductType_ProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionProductType_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionShoppingTime",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false),
                    ShoppingTimeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionShoppingTime", x => new { x.QuestionId, x.ShoppingTimeId });
                    table.ForeignKey(
                        name: "FK_QuestionShoppingTime_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionShoppingTime_ShoppingTime_ShoppingTimeId",
                        column: x => x.ShoppingTimeId,
                        principalTable: "ShoppingTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCategory_CategoryId",
                table: "QuestionCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOnlineShopping_OnlineId",
                table: "QuestionOnlineShopping",
                column: "OnlineId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionPreferredStyle_PreferredId",
                table: "QuestionPreferredStyle",
                column: "PreferredId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionProductType_ProductTypeId",
                table: "QuestionProductType",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionShoppingTime_ShoppingTimeId",
                table: "QuestionShoppingTime",
                column: "ShoppingTimeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionCategory");

            migrationBuilder.DropTable(
                name: "QuestionOnlineShopping");

            migrationBuilder.DropTable(
                name: "QuestionPreferredStyle");

            migrationBuilder.DropTable(
                name: "QuestionProductType");

            migrationBuilder.DropTable(
                name: "QuestionShoppingTime");

            migrationBuilder.DropTable(
                name: "OnlineShopping");

            migrationBuilder.DropTable(
                name: "PreferredStyle");

            migrationBuilder.DropTable(
                name: "ProductType");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "ShoppingTime");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "TBL_User");
        }
    }
}
