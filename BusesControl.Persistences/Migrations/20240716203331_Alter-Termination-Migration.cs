using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterTerminationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terminations_Contracts_ContractId",
                table: "Terminations");

            migrationBuilder.DropForeignKey(
                name: "FK_Terminations_Contracts_ContractModelId",
                table: "Terminations");

            migrationBuilder.DropIndex(
                name: "IX_Terminations_ContractModelId",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "ContractModelId",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Terminations");

            migrationBuilder.AddForeignKey(
                name: "FK_Terminations_Contracts_ContractId",
                table: "Terminations",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terminations_Contracts_ContractId",
                table: "Terminations");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractModelId",
                table: "Terminations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Terminations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terminations_ContractModelId",
                table: "Terminations",
                column: "ContractModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Terminations_Contracts_ContractId",
                table: "Terminations",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Terminations_Contracts_ContractModelId",
                table: "Terminations",
                column: "ContractModelId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }
    }
}
