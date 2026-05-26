using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExportMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("5f003e4f-db17-45b3-be86-9a25fd2575aa"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("74e8fb55-d6e0-48ca-afcc-11b98d489834"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("976b1662-3296-4c78-9d4a-4c819bd3cade"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("a4846d5f-55a7-4bc3-b52f-bd50d9856593"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("ac7d9f89-1db5-46ab-95c3-b30c7c0ce8c3"));

            migrationBuilder.CreateTable(
                name: "Exports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExportedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exports", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FeatureFlags",
                columns: new[] { "Id", "CreatedAt", "Enabled", "Expiration", "Key", "Name" },
                values: new object[,]
                {
                    { new Guid("043d8647-27d8-475f-865b-7ca006f49f4e"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-overdue-invoice-processing", "Processamento de faturas vencidas" },
                    { new Guid("4f810771-5945-46ee-86e9-0b9f3cff186a"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-contract-finalization", "Finalização automática de contratos" },
                    { new Guid("70cb67f1-2ce3-47e3-802c-b8ba144a1aac"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-payment", "Processamento de pagamentos automáticos" },
                    { new Guid("a6bf0ab3-580f-4713-8b62-1c0655a83a3e"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-change-webhook", "Atualização automática de webhooks" },
                    { new Guid("c0e27d51-cd40-47ff-be6c-8b35affa06d6"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-cancel-process-termination", "Cancelamento e encerramento de processos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exports");

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("043d8647-27d8-475f-865b-7ca006f49f4e"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("4f810771-5945-46ee-86e9-0b9f3cff186a"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("70cb67f1-2ce3-47e3-802c-b8ba144a1aac"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("a6bf0ab3-580f-4713-8b62-1c0655a83a3e"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("c0e27d51-cd40-47ff-be6c-8b35affa06d6"));

            migrationBuilder.InsertData(
                table: "FeatureFlags",
                columns: new[] { "Id", "CreatedAt", "Enabled", "Expiration", "Key", "Name" },
                values: new object[,]
                {
                    { new Guid("5f003e4f-db17-45b3-be86-9a25fd2575aa"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-payment", "Processamento de pagamentos automáticos" },
                    { new Guid("74e8fb55-d6e0-48ca-afcc-11b98d489834"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-cancel-process-termination", "Cancelamento e encerramento de processos" },
                    { new Guid("976b1662-3296-4c78-9d4a-4c819bd3cade"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-overdue-invoice-processing", "Processamento de faturas vencidas" },
                    { new Guid("a4846d5f-55a7-4bc3-b52f-bd50d9856593"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-change-webhook", "Atualização automática de webhooks" },
                    { new Guid("ac7d9f89-1db5-46ab-95c3-b30c7c0ce8c3"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-contract-finalization", "Finalização automática de contratos" }
                });
        }
    }
}
