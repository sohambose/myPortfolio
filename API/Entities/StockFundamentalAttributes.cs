using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class StockFundamentalAttributes
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StockFundamentalAttributeID { get; set; }
        public int stockID { get; set; }
        public Stock stock { get; set; }
        public string Statement { get; set; }
        public string Head { get; set; }
        public string Y9 { get; set; }
        public string Y8 { get; set; }
        public string Y7 { get; set; }
        public string Y6 { get; set; }
        public string Y5 { get; set; }
        public string Y4 { get; set; }
        public string Y3 { get; set; }
        public string Y2 { get; set; }
        public string Y1 { get; set; }
        public string Y0 { get; set; }
        public DateTime RecordTimeStamp { get; set; }
    }
}