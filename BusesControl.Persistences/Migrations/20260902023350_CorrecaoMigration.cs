using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { new Guid("70cb67f1-2ce3-47e3-892c-b8ba144a1aac"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-payment", "Processamento de pagamentos automáticos" },
                    { new Guid("9777feed-16c5-436f-ab2e-8d7ee1765475"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-remove-export-expired", "Remove exportações expiradas do sistema e do azure" },
                    { new Guid("a6bfe8a3-580f-4713-8b62-1c06558a83ae"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-change-webhook", "Atualização automática de webhooks" },
                    { new Guid("c0e27d51-cd40-47ff-be6c-8b35afaa6d6d"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-cancel-process-termination", "Cancelamento e encerramento de processos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("70cb67f1-2ce3-47e3-892c-b8ba144a1aac"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("9777feed-16c5-436f-ab2e-8d7ee1765475"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("a6bfe8a3-580f-4713-8b62-1c06558a83ae"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("c0e27d51-cd40-47ff-be6c-8b35afaa6d6d"));

            migrationBuilder.InsertData(
                table: "FeatureFlags",
                columns: new[] { "Id", "CreatedAt", "Enabled", "Expiration", "Key", "Name" },
                values: new object[,]
                {
                    { new Guid("70cb67f1-2ce3-47e3-802c-b8ba144a1aac"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-payment", "Processamento de pagamentos automáticos" },
                    { new Guid("a6bf0ab3-580f-4713-8b62-1c0655a83a3e"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-change-webhook", "Atualização automática de webhooks" },
                    { new Guid("c0e27d51-cd40-47ff-be6c-8b35affa06d6"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-cancel-process-termination", "Cancelamento e encerramento de processos" }
                });
        }
    }
}
