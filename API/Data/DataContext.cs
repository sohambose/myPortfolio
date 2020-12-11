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
    }
}