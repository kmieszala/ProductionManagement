using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class AddTableOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Tanks_TankId",
                        column: x => x.TankId,
                        principalTable: "Tanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2022, 9, 30, 22, 40, 48, 850, DateTimeKind.Local).AddTicks(3429), new DateTime(2022, 9, 30, 22, 40, 48, 850, DateTimeKind.Local).AddTicks(3462) });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TankId",
                table: "Orders",
                column: "TankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2022, 9, 16, 22, 5, 31, 136, DateTimeKind.Local).AddTicks(8605), new DateTime(2022, 9, 16, 22, 5, 31, 136, DateTimeKind.Local).AddTicks(8650) });
        }
    }
}
