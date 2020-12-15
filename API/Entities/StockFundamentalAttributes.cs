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
        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y9 { get; set; }
        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y8 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y7 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y6 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y5 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y4 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y3 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y2 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y1 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Y0 { get; set; }
        public DateTime RecordTimeStamp { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal? observationValue { get; set; }
        public string observationValueType { get; set; }
    }
}