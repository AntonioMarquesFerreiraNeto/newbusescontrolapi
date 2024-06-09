using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserRegistrationQueue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrationQueueModels_EmployeeId",
                table: "UserRegistrationQueueModels",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRegistrationQueueModels_Employees_EmployeeId",
                table: "UserRegistrationQueueModels",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRegistrationQueueModels_Employees_EmployeeId",
                table: "UserRegistrationQueueModels");

            migrationBuilder.DropIndex(
                name: "IX_UserRegistrationQueueModels_EmployeeId",
                table: "UserRegistrationQueueModels");
        }
    }
}
