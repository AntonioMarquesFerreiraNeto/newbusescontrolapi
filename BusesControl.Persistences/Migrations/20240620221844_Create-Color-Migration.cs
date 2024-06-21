using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateColorMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Colors_ColorModelId",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_ColorModelId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "ColorModelId",
                table: "Buses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ColorModelId",
                table: "Buses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buses_ColorModelId",
                table: "Buses",
                column: "ColorModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Colors_ColorModelId",
                table: "Buses",
                column: "ColorModelId",
                principalTable: "Colors",
                principalColumn: "Id");
        }
    }
}
