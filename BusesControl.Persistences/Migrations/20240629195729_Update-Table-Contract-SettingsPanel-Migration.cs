using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableContractSettingsPanelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SettingsPanelId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SettingsPanelId",
                table: "Contracts",
                column: "SettingsPanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_SettingsPanel_SettingsPanelId",
                table: "Contracts",
                column: "SettingsPanelId",
                principalTable: "SettingsPanel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_SettingsPanel_SettingsPanelId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_SettingsPanelId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "SettingsPanelId",
                table: "Contracts");
        }
    }
}
