using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRegistrationQueueModels_Employees_EmployeeId",
                table: "UserRegistrationQueueModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRegistrationQueueModels",
                table: "UserRegistrationQueueModels");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserRegistrationQueueModels");

            migrationBuilder.RenameTable(
                name: "UserRegistrationQueueModels",
                newName: "UsersRegistrationQueue");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "UsersRegistrationQueue",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_UserRegistrationQueueModels_EmployeeId",
                table: "UsersRegistrationQueue",
                newName: "IX_UsersRegistrationQueue_EmployeeId");

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedId",
                table: "UsersRegistrationQueue",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UsersRegistrationQueue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UsersRegistrationQueue",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRegistrationQueue",
                table: "UsersRegistrationQueue",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsersRegistrationSecurityCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRegistrationSecurityCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersRegistrationSecurityCode_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersRegistrationSecurityCode_UserId",
                table: "UsersRegistrationSecurityCode",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRegistrationQueue_Employees_EmployeeId",
                table: "UsersRegistrationQueue",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersRegistrationQueue_Employees_EmployeeId",
                table: "UsersRegistrationQueue");

            migrationBuilder.DropTable(
                name: "UsersRegistrationSecurityCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRegistrationQueue",
                table: "UsersRegistrationQueue");

            migrationBuilder.DropColumn(
                name: "ApprovedId",
                table: "UsersRegistrationQueue");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UsersRegistrationQueue");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsersRegistrationQueue");

            migrationBuilder.RenameTable(
                name: "UsersRegistrationQueue",
                newName: "UserRegistrationQueueModels");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserRegistrationQueueModels",
                newName: "RequestDate");

            migrationBuilder.RenameIndex(
                name: "IX_UsersRegistrationQueue_EmployeeId",
                table: "UserRegistrationQueueModels",
                newName: "IX_UserRegistrationQueueModels_EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "UserRegistrationQueueModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRegistrationQueueModels",
                table: "UserRegistrationQueueModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRegistrationQueueModels_Employees_EmployeeId",
                table: "UserRegistrationQueueModels",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
