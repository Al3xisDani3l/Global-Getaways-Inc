using Microsoft.EntityFrameworkCore.Migrations;

namespace GG.Data.Migrations
{
    public partial class Correcion_state_State : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "state",
                table: "TravelPackages",
                newName: "State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "TravelPackages",
                newName: "state");
        }
    }
}
