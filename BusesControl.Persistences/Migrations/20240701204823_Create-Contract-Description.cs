using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateContractDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_SettingsPanel_SettingsPanelId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "SettingsPanelId",
                table: "Contracts",
                newName: "SettingPanelId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_SettingsPanelId",
                table: "Contracts",
                newName: "IX_Contracts_SettingPanelId");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractDescriptionId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ContractDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    GeneralProvisions = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Objective = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Copyright = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDescriptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractDescriptionId",
                table: "Contracts",
                column: "ContractDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractDescriptions_ContractDescriptionId",
                table: "Contracts",
                column: "ContractDescriptionId",
                principalTable: "ContractDescriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_SettingsPanel_SettingPanelId",
                table: "Contracts",
                column: "SettingPanelId",
                principalTable: "SettingsPanel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractDescriptions_ContractDescriptionId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_SettingsPanel_SettingPanelId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "ContractDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractDescriptionId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractDescriptionId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "SettingPanelId",
                table: "Contracts",
                newName: "SettingsPanelId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_SettingPanelId",
                table: "Contracts",
                newName: "IX_Contracts_SettingsPanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_SettingsPanel_SettingsPanelId",
                table: "Contracts",
                column: "SettingsPanelId",
                principalTable: "SettingsPanel",
                principalColumn: "Id");
        }
    }
}
