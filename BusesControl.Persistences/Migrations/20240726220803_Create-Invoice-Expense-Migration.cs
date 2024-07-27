using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateInvoiceExpenseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Financials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvoicesExpense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    FinancialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicesExpense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoicesExpense_Financials_FinancialId",
                        column: x => x.FinancialId,
                        principalTable: "Financials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Financials_SupplierId",
                table: "Financials",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesExpense_FinancialId",
                table: "InvoicesExpense",
                column: "FinancialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financials_Suppliers_SupplierId",
                table: "Financials",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financials_Suppliers_SupplierId",
                table: "Financials");

            migrationBuilder.DropTable(
                name: "InvoicesExpense");

            migrationBuilder.DropIndex(
                name: "IX_Financials_SupplierId",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Financials");
        }
    }
}
