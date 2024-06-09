using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableResetPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetsUser_Users_UserId",
                table: "ResetsUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResetsUser",
                table: "ResetsUser");

            migrationBuilder.RenameTable(
                name: "ResetsUser",
                newName: "ResetPasswordsSecurityCode");

            migrationBuilder.RenameIndex(
                name: "IX_ResetsUser_UserId",
                table: "ResetPasswordsSecurityCode",
                newName: "IX_ResetPasswordsSecurityCode_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResetPasswordsSecurityCode",
                table: "ResetPasswordsSecurityCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetPasswordsSecurityCode_Users_UserId",
                table: "ResetPasswordsSecurityCode",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetPasswordsSecurityCode_Users_UserId",
                table: "ResetPasswordsSecurityCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResetPasswordsSecurityCode",
                table: "ResetPasswordsSecurityCode");

            migrationBuilder.RenameTable(
                name: "ResetPasswordsSecurityCode",
                newName: "ResetsUser");

            migrationBuilder.RenameIndex(
                name: "IX_ResetPasswordsSecurityCode_UserId",
                table: "ResetsUser",
                newName: "IX_ResetsUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResetsUser",
                table: "ResetsUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetsUser_Users_UserId",
                table: "ResetsUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
