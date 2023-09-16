using Microsoft.EntityFrameworkCore.Migrations;

namespace FlairTickets.Web.Migrations
{
    public partial class AddTicketIndexFlightSeatUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightId_Seat",
                table: "Tickets",
                columns: new[] { "FlightId", "Seat" },
                unique: true,
                filter: "[FlightId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightId_Seat",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightId",
                table: "Tickets",
                column: "FlightId");
        }
    }
}
