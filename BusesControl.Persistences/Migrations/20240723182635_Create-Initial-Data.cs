using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusesControl.Persistence.Migrations;

/// <inheritdoc />
public partial class CreateInitialData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        Guid employeeAdminId = Guid.Parse("12d50345-3c20-4c49-fda8-08dc88d88383");
        Guid employeeAssistantId = Guid.Parse("8dcfffbc-2a48-49f9-98b9-08dc9d62917f");

        Guid adminUserId = Guid.Parse("124868ad-70bb-4606-b225-49e457b0ff4c");
        Guid assistantUserId = Guid.Parse("4ddc436e-0daf-423a-9d69-dae899fdb359");
        Guid systemUserId = Guid.Parse("a8e3e2b8-7d1c-4d22-a713-a63d3cc4259f");

        Guid adminRoleId = Guid.Parse("0300add6-fdd9-497e-b400-5111fc804434");
        Guid assistantRoleId = Guid.Parse("6c84509b-9c38-492b-b524-6afffabfabee");
        Guid systemRoleId = Guid.Parse("8fe66851-2d59-4c40-a219-130cb8bc1b05");

        migrationBuilder.InsertData("Roles",
        columns:
        [
            "Id",
            "Name",
            "NormalizedName",
            "ConcurrencyStamp"
        ],
        values: new object[,]
        {
            { adminRoleId, "Admin", "ADMIN", "c332220a-479c-4029-90f8-eaae330a9141" },
            { assistantRoleId, "Assistant", "ASSISTANT", "ab879eb0-e677-4c61-bc6a-c2d9a37312e2" },
            { systemRoleId, "System", "SYSTEM", "0db578bb-2121-4f98-84a3-3a7751d894e3" }
        });

        migrationBuilder.InsertData(table: "Employees",
        columns:
        [
            "Id",
            "Name",
            "Cpf",
            "BirthDate",
            "Email",
            "PhoneNumber",
            "HomeNumber",
            "Logradouro",
            "ComplementResidential",
            "Neighborhood",
            "City",
            "State",
            "Type",
            "Status"
        ],
        values: new object[,]
        {
            { employeeAdminId, "Antonio Marques Admin", "29125064096", "1990-01-01", "antonio@gmail.com", "1234567890", "42", "Rua das Flores", "Apto 101", "Centro", "São Paulo", "SP", 3, 1 },
            { employeeAssistantId, "Raquel Letícia da Cunha", "80898354307", "2001-05-15", "frontendbackend006@gmail.com", "11982345678", "200", "Avenida Principal, 456", "Casa 2", "Bairro Central", "São Paulo", "SP", 2, 1 }
        });

        migrationBuilder.InsertData("Users",
        columns:
        [
            "Id",
            "EmployeeId",
            "Nickname",
            "PhoneNumber",
            "Status",
            "UserName",
            "NormalizedUserName",
            "Email",
            "NormalizedEmail",
            "EmailConfirmed",
            "PasswordHash",
            "SecurityStamp",
            "ConcurrencyStamp",
            "PhoneNumberConfirmed",
            "TwoFactorEnabled",
            "LockoutEnd",
            "LockoutEnabled",
            "AccessFailedCount"
        ],
        values: new object[,]
        {
            { adminUserId, employeeAdminId, "Antonio - Admin", "62972345678", 1, "antonio@gmail.com", "ANTONIO@GMAIL.COM", "antonio@gmail.com", "ANTONIO@GMAIL.COM", false, "AQAAAAIAAYagAAAAEEPUjzRSY6IzySo5GfpBpndAcHn0VbULkQbcP4Nk0KrfKPfmQiGYF5MAbdSbLFFfcg==", "SK2DLQLAT2KYGIC6XHSSG7OCZ77DXSFI", "1ca82e4c-8084-4302-88a3-0fe98ea23dab", false, false, null, true, 0 },
            { assistantUserId, employeeAssistantId, "Raquel - Assistant", "11982345678", 1, "frontendbackend006@gmail.com", "FRONTENDBACKEND006@GMAIL.COM", "frontendbackend006@gmail.com", "FRONTENDBACKEND006@GMAIL.COM", false, "AQAAAAIAAYagAAAAEEPUjzRSY6IzySo5GfpBpndAcHn0VbULkQbcP4Nk0KrfKPfmQiGYF5MAbdSbLFFfcg==", "SSIWEH5OXGTVFTEE52QYFWLZIYWIAS4L", "3cd3faee-dcee-4efa-8d5c-7e7916661a89", false, false, null, true, 0 },
            { systemUserId, null, "Sistema - System", "11982345679", 1, "system@gmail.com", "SYSTEM@GMAIL.COM", "system@gmail.com", "SYSTEM@GMAIL.COM", false, "AQAAAAIAAYagAAAAEEPUjzRSY6IzySo5GfpBpndAcHn0VbULkQbcP4Nk0KrfKPfmQiGYF5MAbdSbLFFfcg==", "KNJ3L72YDP3R5KTGFVO7JIKHYD28J4YK", "9d8c6eab-3c5a-4e2d-a2a7-f2d29d9e899c", false, false, null, true, 0 }
        });

        migrationBuilder.InsertData("UserRoles",
            columns:
            [
                "UserId",
                "RoleId"
            ],
            values: new object[,]
            {
                { adminUserId, adminRoleId },
                { assistantUserId, assistantRoleId },
                { systemUserId, systemRoleId }
            }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM \"UserRoles\"");
        migrationBuilder.Sql("DELETE FROM \"Roles\"");
        migrationBuilder.Sql("DELETE FROM \"Users\"");
        migrationBuilder.Sql("DELETE FROM \"Employees\"");
    }
}