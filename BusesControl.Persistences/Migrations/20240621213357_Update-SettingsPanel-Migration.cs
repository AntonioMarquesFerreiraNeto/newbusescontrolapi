using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettingsPanelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SettingsPanel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "RequesterId",
                table: "SettingsPanel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SettingsPanel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettingsPanel_RequesterId",
                table: "SettingsPanel",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SettingsPanel_Users_RequesterId",
                table: "SettingsPanel",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingsPanel_Users_RequesterId",
                table: "SettingsPanel");

            migrationBuilder.DropIndex(
                name: "IX_SettingsPanel_RequesterId",
                table: "SettingsPanel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SettingsPanel");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "SettingsPanel");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SettingsPanel");
        }
    }
}
