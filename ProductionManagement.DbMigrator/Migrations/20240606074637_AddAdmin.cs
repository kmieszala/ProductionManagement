using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class AddAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivationDate", "Email", "FirstName", "LastName", "Password", "RegisteredDate", "Status", "TimeBlockCount" },
                values: new object[] { 1, new DateTime(2024, 6, 6, 9, 46, 37, 71, DateTimeKind.Local).AddTicks(9736), "", "Admin", "Admin", "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9", new DateTime(2024, 6, 6, 9, 46, 37, 71, DateTimeKind.Local).AddTicks(9779), 2, 0 });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[] { 1, 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
