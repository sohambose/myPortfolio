namespace API.DTOS
{
    public class StockComparisonDetailsDTO
    {
        public string Head { get; set; }
        public string Statement { get; set; }
        public int stockID { get; set; }

        public string stockSymbol { get; set; }
        public decimal ObservationValue { get; set; }
        public string ObservationValueType { get; set; }
        public decimal StockScore { get; set; }
    }
}