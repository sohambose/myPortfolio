using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class RenameColumnCasing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockSymbol",
                table: "Stocks",
                newName: "stockSymbol");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Stocks",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Industry",
                table: "Stocks",
                newName: "industry");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Stocks",
                newName: "companyName");

            migrationBuilder.RenameColumn(
                name: "StockID",
                table: "Stocks",
                newName: "stockID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "stockSymbol",
                table: "Stocks",
                newName: "StockSymbol");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Stocks",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "industry",
                table: "Stocks",
                newName: "Industry");

            migrationBuilder.RenameColumn(
                name: "companyName",
                table: "Stocks",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "stockID",
                table: "Stocks",
                newName: "StockID");
        }
    }
}
