using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFactoryERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorGoodsReceiptEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedBy",
                schema: "Purchasing",
                table: "GoodsReceipts");

            migrationBuilder.AddColumn<int>(
                name: "ReceivedById",
                schema: "Purchasing",
                table: "GoodsReceipts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_ReceivedById",
                schema: "Purchasing",
                table: "GoodsReceipts",
                column: "ReceivedById");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceipts_Employees_ReceivedById",
                schema: "Purchasing",
                table: "GoodsReceipts",
                column: "ReceivedById",
                principalSchema: "HR",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceipts_Employees_ReceivedById",
                schema: "Purchasing",
                table: "GoodsReceipts");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipts_ReceivedById",
                schema: "Purchasing",
                table: "GoodsReceipts");

            migrationBuilder.DropColumn(
                name: "ReceivedById",
                schema: "Purchasing",
                table: "GoodsReceipts");

            migrationBuilder.AddColumn<string>(
                name: "ReceivedBy",
                schema: "Purchasing",
                table: "GoodsReceipts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
