using Microsoft.EntityFrameworkCore.Migrations;

namespace FishStore.Data.Migrations
{
    public partial class OrderHeaderv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderHeader",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "OrderHeader",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "City",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "State",
                table: "OrderHeader");
        }
    }
}
