using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class StockComparisonTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockComparisonScores",
                columns: table => new
                {
                    StockComparisonScoresID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Head = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stockID = table.Column<int>(type: "int", nullable: false),
                    ObservationValue = table.Column<decimal>(type: "decimal(20,5)", nullable: false),
                    ObservationValueType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockScore = table.Column<decimal>(type: "decimal(20,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockComparisonScores", x => x.StockComparisonScoresID);
                    table.ForeignKey(
                        name: "FK_StockComparisonScores_Stocks_stockID",
                        column: x => x.stockID,
                        principalTable: "Stocks",
                        principalColumn: "stockID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockComparisonScores_stockID",
                table: "StockComparisonScores",
                column: "stockID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockComparisonScores");
        }
    }
}
