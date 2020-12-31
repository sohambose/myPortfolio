using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class StockComparisonScores
    {
        public int StockComparisonScoresID { get; set; }
        public string Head { get; set; }
        public int stockID { get; set; }
        public Stock stock { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal ObservationValue { get; set; }
        public string ObservationValueType { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal StockScore { get; set; }
    }
}