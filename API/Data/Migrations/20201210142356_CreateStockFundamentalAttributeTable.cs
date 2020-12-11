using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class CreateStockFundamentalAttributeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockFundamentalAttributes",
                columns: table => new
                {
                    StockFundamentalAttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stockID = table.Column<int>(type: "int", nullable: false),
                    Statement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Head = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Y0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockFundamentalAttributes", x => x.StockFundamentalAttributeID);
                    table.ForeignKey(
                        name: "FK_StockFundamentalAttributes_Stocks_stockID",
                        column: x => x.stockID,
                        principalTable: "Stocks",
                        principalColumn: "stockID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockFundamentalAttributes_stockID",
                table: "StockFundamentalAttributes",
                column: "stockID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockFundamentalAttributes");
        }
    }
}
