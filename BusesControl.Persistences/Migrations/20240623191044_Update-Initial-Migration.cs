using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingsPanel_Users_RequesterId",
                table: "SettingsPanel");

            migrationBuilder.AddColumn<int>(
                name: "LimitDateTermination",
                table: "SettingsPanel",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SettingsPanel_Employees_RequesterId",
                table: "SettingsPanel",
                column: "RequesterId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingsPanel_Employees_RequesterId",
                table: "SettingsPanel");

            migrationBuilder.DropColumn(
                name: "LimitDateTermination",
                table: "SettingsPanel");

            migrationBuilder.AddForeignKey(
                name: "FK_SettingsPanel_Users_RequesterId",
                table: "SettingsPanel",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
