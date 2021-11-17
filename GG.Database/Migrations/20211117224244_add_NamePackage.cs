using Microsoft.EntityFrameworkCore.Migrations;

namespace GG.Data.Migrations
{
    public partial class add_NamePackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NamePackage",
                table: "TravelPackages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NamePackage",
                table: "TravelPackages");
        }
    }
}
