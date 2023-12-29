using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlairTickets.Web.Migrations
{
    public partial class AddPhotoToAirplane : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PhotoGuid",
                table: "Airplanes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoGuid",
                table: "Airplanes");
        }
    }
}
