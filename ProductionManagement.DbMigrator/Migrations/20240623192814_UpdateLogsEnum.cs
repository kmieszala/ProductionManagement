using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class UpdateLogsEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LogCodeDict",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "LogCodeDict",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[,]
                {
                    { 7, true, "MarkOrderAsDone_BadCode" },
                    { 8, true, "MarkOrderAsDone_NoOrder" },
                    { 9, true, "MarkOrderAsDone_OrderCompleted" },
                    { 10, true, "MarkOrderAsDone_NextOrderInProgress" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LogCodeDict",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LogCodeDict",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LogCodeDict",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LogCodeDict",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LogCodeDict",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

        }
    }
}
