using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class ChangeStockFAColumnstoDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Y9",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y8",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y7",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y6",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y5",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y4",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y3",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y2",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y1",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Y0",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Y9",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y8",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y7",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y6",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y5",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y4",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y3",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y2",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y1",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");

            migrationBuilder.AlterColumn<string>(
                name: "Y0",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");
        }
    }
}
