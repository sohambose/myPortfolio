using API.Entities;

namespace API.DTOS
{
    public class StockScoreDTO
    {
        public int stockID { get; set; }
        public string StockName { get; set; }
        public decimal Score { get; set; }
    }
}