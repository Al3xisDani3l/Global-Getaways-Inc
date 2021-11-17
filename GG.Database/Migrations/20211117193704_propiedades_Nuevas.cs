using Microsoft.EntityFrameworkCore.Migrations;

namespace GG.Data.Migrations
{
    public partial class propiedades_Nuevas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KindUser",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImg",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "TravelPackages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImg",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "state",
                table: "TravelPackages");

            migrationBuilder.AddColumn<int>(
                name: "KindUser",
                table: "Users",
                type: "int",
                nullable: true);
        }
    }
}
