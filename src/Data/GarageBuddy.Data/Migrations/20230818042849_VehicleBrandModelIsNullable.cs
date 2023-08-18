using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageBuddy.Data.Migrations
{
    public partial class VehicleBrandModelIsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_BrandModels_BrandModelId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandModelId",
                table: "Vehicles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_BrandModels_BrandModelId",
                table: "Vehicles",
                column: "BrandModelId",
                principalTable: "BrandModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_BrandModels_BrandModelId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandModelId",
                table: "Vehicles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_BrandModels_BrandModelId",
                table: "Vehicles",
                column: "BrandModelId",
                principalTable: "BrandModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
