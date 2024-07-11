using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFinancialInvoiceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Contracts",
                newName: "PaymentType");

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Financials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Financials");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Contracts",
                newName: "PaymentMethod");
        }
    }
}
