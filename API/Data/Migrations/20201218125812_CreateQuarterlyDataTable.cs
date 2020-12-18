using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class CreateQuarterlyDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockQuarterlyData",
                columns: table => new
                {
                    StockQuarterlyDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stockID = table.Column<int>(type: "int", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q9 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q8 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q7 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q6 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q5 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q4 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q3 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q2 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q1 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    Q0 = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    observationValue = table.Column<decimal>(type: "decimal(20,5)", nullable: true),
                    observationValueType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockQuarterlyData", x => x.StockQuarterlyDataID);
                    table.ForeignKey(
                        name: "FK_StockQuarterlyData_Stocks_stockID",
                        column: x => x.stockID,
                        principalTable: "Stocks",
                        principalColumn: "stockID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockQuarterlyData_stockID",
                table: "StockQuarterlyData",
                column: "stockID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockQuarterlyData");
        }
    }
}
