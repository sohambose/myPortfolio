using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class AddObservationValueCols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "observationValue",
                table: "StockFundamentalAttributes",
                type: "decimal(20,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "observationValueType",
                table: "StockFundamentalAttributes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "observationValue",
                table: "StockFundamentalAttributes");

            migrationBuilder.DropColumn(
                name: "observationValueType",
                table: "StockFundamentalAttributes");
        }
    }
}
