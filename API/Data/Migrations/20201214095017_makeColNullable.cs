using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class makeColNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "observationValue",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "observationValue",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,5)",
                oldNullable: true);
        }
    }
}
