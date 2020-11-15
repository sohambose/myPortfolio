using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Stock
    {
        public int StockID { get; set; }
        [Required]
        public string StockSymbol { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Industry { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}