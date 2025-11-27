using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("1ef1e157-8b7f-453f-ab99-32965c1a2252"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("20d2885a-838f-4bc6-bde6-aecbedb85f61"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("59f48749-660f-4001-89a0-a5295413be74"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("a8fc2827-b40c-45dc-b0d0-2bb74a27894e"));

            migrationBuilder.DeleteData(
                table: "FeatureFlags",
                keyColumn: "Id",
                keyValue: new Guid("c38e57ed-6130-4896-becf-7eed8f472c0f"));

            migrationBuilder.CreateTable(
                name: "TwoFas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IpLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoFas", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwoFas");

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

            migrationBuilder.InsertData(
                table: "FeatureFlags",
                columns: new[] { "Id", "CreatedAt", "Enabled", "Expiration", "Key", "Name" },
                values: new object[,]
                {
                    { new Guid("1ef1e157-8b7f-453f-ab99-32965c1a2252"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-contract-finalization", "Finalização automática de contratos" },
                    { new Guid("20d2885a-838f-4bc6-bde6-aecbedb85f61"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-change-webhook", "Atualização automática de webhooks" },
                    { new Guid("59f48749-660f-4001-89a0-a5295413be74"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-cancel-process-termination", "Cancelamento e encerramento de processos" },
                    { new Guid("a8fc2827-b40c-45dc-b0d0-2bb74a27894e"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-payment", "Processamento de pagamentos automáticos" },
                    { new Guid("c38e57ed-6130-4896-becf-7eed8f472c0f"), new DateTime(2025, 8, 9, 14, 49, 0, 0, DateTimeKind.Unspecified), true, null, "automated-overdue-invoice-processing", "Processamento de faturas vencidas" }
                });
        }
    }
}
