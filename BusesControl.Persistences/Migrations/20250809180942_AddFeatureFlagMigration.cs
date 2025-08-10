using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureFlagMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureFlags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(355)", maxLength: 355, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(355)", maxLength: 355, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureFlags", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureFlags");
        }
    }
}
