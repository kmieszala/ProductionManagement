using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class Parts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2022, 9, 7, 21, 28, 18, 924, DateTimeKind.Local).AddTicks(489), new DateTime(2022, 9, 7, 21, 28, 18, 924, DateTimeKind.Local).AddTicks(527) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2022, 8, 14, 21, 16, 3, 62, DateTimeKind.Local).AddTicks(5157), new DateTime(2022, 8, 14, 21, 16, 3, 62, DateTimeKind.Local).AddTicks(5256) });
        }
    }
}
