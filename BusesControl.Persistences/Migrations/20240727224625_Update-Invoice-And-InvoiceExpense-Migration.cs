using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceAndInvoiceExpenseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "InvoicesExpense",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Invoices",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "InvoicesExpense");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Invoices");
        }
    }
}
