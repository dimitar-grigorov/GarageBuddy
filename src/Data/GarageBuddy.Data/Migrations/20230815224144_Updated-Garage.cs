using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageBuddy.Data.Migrations
{
    public partial class UpdatedGarage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Garages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Garages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Garages_IsDeleted",
                table: "Garages",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Garages_IsDeleted",
                table: "Garages");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Garages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Garages");
        }
    }
}
