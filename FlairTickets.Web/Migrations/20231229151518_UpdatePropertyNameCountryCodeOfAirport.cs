using Microsoft.EntityFrameworkCore.Migrations;

namespace FlairTickets.Web.Migrations
{
    public partial class UpdatePropertyNameCountryCodeOfAirport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountryCode2Digit",
                table: "Airports",
                newName: "CountryCode2Letters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountryCode2Letters",
                table: "Airports",
                newName: "CountryCode2Digit");
        }
    }
}
