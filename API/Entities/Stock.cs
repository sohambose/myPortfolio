using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Stock
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int stockID { get; set; }
        [Required]
        public string stockSymbol { get; set; }
        [Required]
        public string companyName { get; set; }
        [Required]
        public string industry { get; set; }

        [Required]
        public int quantity { get; set; }
    }
}