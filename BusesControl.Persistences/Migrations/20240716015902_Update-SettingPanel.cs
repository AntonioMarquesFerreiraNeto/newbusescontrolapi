using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettingPanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LimitDateTermination",
                table: "SettingsPanel",
                newName: "LimitDateTerminate");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractModelId",
                table: "Terminations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terminations_ContractModelId",
                table: "Terminations",
                column: "ContractModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Terminations_Contracts_ContractModelId",
                table: "Terminations",
                column: "ContractModelId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terminations_Contracts_ContractModelId",
                table: "Terminations");

            migrationBuilder.DropIndex(
                name: "IX_Terminations_ContractModelId",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "ContractModelId",
                table: "Terminations");

            migrationBuilder.RenameColumn(
                name: "LimitDateTerminate",
                table: "SettingsPanel",
                newName: "LimitDateTermination");
        }
    }
}
