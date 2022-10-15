using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.DbMigrator.Migrations
{
    public partial class ProductionDaysConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionDays_Orders_OrdersId",
                table: "ProductionDays");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionDays_ProductionLine_ProductionLineId",
                table: "ProductionDays");

            migrationBuilder.DropIndex(
                name: "IX_ProductionDays_OrdersId",
                table: "ProductionDays");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "ProductionDays");

            migrationBuilder.AlterColumn<int>(
                name: "ProductionLineId",
                table: "ProductionDays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionLineId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductionLineId",
                table: "Orders",
                column: "ProductionLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ProductionLine_ProductionLineId",
                table: "Orders",
                column: "ProductionLineId",
                principalTable: "ProductionLine",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionDays_ProductionLine_ProductionLineId",
                table: "ProductionDays",
                column: "ProductionLineId",
                principalTable: "ProductionLine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ProductionLine_ProductionLineId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionDays_ProductionLine_ProductionLineId",
                table: "ProductionDays");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductionLineId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductionLineId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ProductionLineId",
                table: "ProductionDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OrdersId",
                table: "ProductionDays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionDays_OrdersId",
                table: "ProductionDays",
                column: "OrdersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionDays_Orders_OrdersId",
                table: "ProductionDays",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionDays_ProductionLine_ProductionLineId",
                table: "ProductionDays",
                column: "ProductionLineId",
                principalTable: "ProductionLine",
                principalColumn: "Id");
        }
    }
}
