using Microsoft.EntityFrameworkCore.Migrations;

namespace FishStore.Data.Migrations
{
    public partial class nameChangeAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "OrderHeader");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "OrderHeader",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "OrderHeader");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
