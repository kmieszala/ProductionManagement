using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_Sequence",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ProductionLine",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ProductionLine");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Sequence",
                table: "Orders",
                column: "Sequence",
                unique: true);
        }
    }
}
