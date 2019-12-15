using Microsoft.EntityFrameworkCore.Migrations;

namespace FishStore.Data.Migrations
{
    public partial class StoreItemToDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    SubCategoryId = table.Column<int>(nullable: false),
                    Volume = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreItem_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StoreItem_SubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_CategoryId",
                table: "StoreItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_SubCategoryId",
                table: "StoreItem",
                column: "SubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreItem");
        }
    }
}
