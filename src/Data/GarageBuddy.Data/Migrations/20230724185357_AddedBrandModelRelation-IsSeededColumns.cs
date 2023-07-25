#nullable disable

namespace GarageBuddy.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedBrandModelRelationIsSeededColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeeded",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "BrandModels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsSeeded",
                table: "BrandModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BrandModels_BrandId",
                table: "BrandModels",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandModels_Brands_BrandId",
                table: "BrandModels",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandModels_Brands_BrandId",
                table: "BrandModels");

            migrationBuilder.DropIndex(
                name: "IX_BrandModels_BrandId",
                table: "BrandModels");

            migrationBuilder.DropColumn(
                name: "IsSeeded",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "BrandModels");

            migrationBuilder.DropColumn(
                name: "IsSeeded",
                table: "BrandModels");
        }
    }
}
