using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockFundamentalAttributes> StockFundamentalAttributes { get; set; }
        public DbSet<StockQuarterlyData> StockQuarterlyData { get; set; }
        public DbSet<StockComparisonScores> StockComparisonScores { get; set; }
    }
}