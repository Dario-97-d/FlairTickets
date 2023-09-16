using Microsoft.EntityFrameworkCore.Migrations;

namespace FlairTickets.Web.Migrations
{
    public partial class ChangeForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_DestinationId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_OriginId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightId_Seat",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DestinationId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_OriginId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AirplaneId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestinationAirportId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginAirportId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightId_Seat",
                table: "Tickets",
                columns: new[] { "FlightId", "Seat" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationAirportId",
                table: "Flights",
                column: "DestinationAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_OriginAirportId",
                table: "Flights",
                column: "OriginAirportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_DestinationAirportId",
                table: "Flights",
                column: "DestinationAirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_OriginAirportId",
                table: "Flights",
                column: "OriginAirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_DestinationAirportId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_OriginAirportId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightId_Seat",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DestinationAirportId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_OriginAirportId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DestinationAirportId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "OriginAirportId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AirplaneId",
                table: "Flights",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DestinationId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightId_Seat",
                table: "Tickets",
                columns: new[] { "FlightId", "Seat" },
                unique: true,
                filter: "[FlightId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationId",
                table: "Flights",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_OriginId",
                table: "Flights",
                column: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_DestinationId",
                table: "Flights",
                column: "DestinationId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_OriginId",
                table: "Flights",
                column: "OriginId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
