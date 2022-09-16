using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class LineTank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LineTank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TankId = table.Column<int>(type: "int", nullable: false),
                    ProductionLineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineTank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineTank_ProductionLine_ProductionLineId",
                        column: x => x.ProductionLineId,
                        principalTable: "ProductionLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineTank_Tanks_TankId",
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
                values: new object[] { new DateTime(2022, 9, 16, 22, 5, 31, 136, DateTimeKind.Local).AddTicks(8605), new DateTime(2022, 9, 16, 22, 5, 31, 136, DateTimeKind.Local).AddTicks(8650) });

            migrationBuilder.CreateIndex(
                name: "IX_LineTank_ProductionLineId",
                table: "LineTank",
                column: "ProductionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_LineTank_TankId",
                table: "LineTank",
                column: "TankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineTank");

            migrationBuilder.DropTable(
                name: "ProductionLine");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ActivationDate", "RegisteredDate" },
                values: new object[] { new DateTime(2022, 9, 15, 22, 21, 52, 225, DateTimeKind.Local).AddTicks(7149), new DateTime(2022, 9, 15, 22, 21, 52, 225, DateTimeKind.Local).AddTicks(7208) });
        }
    }
}
