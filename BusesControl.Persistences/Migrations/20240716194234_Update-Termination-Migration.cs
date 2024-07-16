using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTerminationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terminations_Financials_FinancialId",
                table: "Terminations");

            migrationBuilder.DropIndex(
                name: "IX_Terminations_FinancialId",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "FinancialId",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Terminations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FinancialId",
                table: "Terminations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Terminations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Terminations_FinancialId",
                table: "Terminations",
                column: "FinancialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Terminations_Financials_FinancialId",
                table: "Terminations",
                column: "FinancialId",
                principalTable: "Financials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
