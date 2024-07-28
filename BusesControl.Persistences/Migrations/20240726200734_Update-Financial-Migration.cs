using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFinancialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Financials",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SettingPanelId",
                table: "Financials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Financials_SettingPanelId",
                table: "Financials",
                column: "SettingPanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financials_SettingsPanel_SettingPanelId",
                table: "Financials",
                column: "SettingPanelId",
                principalTable: "SettingsPanel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financials_SettingsPanel_SettingPanelId",
                table: "Financials");

            migrationBuilder.DropIndex(
                name: "IX_Financials_SettingPanelId",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "SettingPanelId",
                table: "Financials");
        }
    }
}
