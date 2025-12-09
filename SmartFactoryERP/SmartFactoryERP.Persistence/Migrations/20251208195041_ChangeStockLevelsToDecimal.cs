using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFactoryERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStockLevelsToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStockLevel",
                schema: "Inventory",
                table: "Materials",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentStockLevel",
                schema: "Inventory",
                table: "Materials",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MinimumStockLevel",
                schema: "Inventory",
                table: "Materials",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentStockLevel",
                schema: "Inventory",
                table: "Materials",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
