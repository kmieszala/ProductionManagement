using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class UserSecretCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Users",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "0001");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[] { 9, true, "PodgldZlecen" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "Code", "RegisteredDate" },
                values: new object[] { new DateTime(2024, 6, 12, 10, 46, 19, 464, DateTimeKind.Local).AddTicks(5030), "0807", new DateTime(2024, 6, 12, 10, 46, 19, 464, DateTimeKind.Local).AddTicks(5067) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Code",
                table: "Users",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Code",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2024, 6, 6, 9, 46, 37, 71, DateTimeKind.Local).AddTicks(9736), new DateTime(2024, 6, 6, 9, 46, 37, 71, DateTimeKind.Local).AddTicks(9779) });
        }
    }
}
