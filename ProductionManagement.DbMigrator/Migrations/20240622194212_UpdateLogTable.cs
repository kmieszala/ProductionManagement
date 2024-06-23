using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class UpdateLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Log",
                newName: "CreationDate");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Log",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2024, 6, 22, 21, 42, 11, 757, DateTimeKind.Local).AddTicks(3720), new DateTime(2024, 6, 22, 21, 42, 11, 757, DateTimeKind.Local).AddTicks(3752) });

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserId",
                table: "Log",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Users_UserId",
                table: "Log",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Users_UserId",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_UserId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Log",
                newName: "Date");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2024, 6, 22, 21, 20, 54, 593, DateTimeKind.Local).AddTicks(3949), new DateTime(2024, 6, 22, 21, 20, 54, 593, DateTimeKind.Local).AddTicks(4002) });
        }
    }
}
