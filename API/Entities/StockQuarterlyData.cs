using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class StockQuarterlyData
    {

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StockQuarterlyDataID { get; set; }
        public int stockID { get; set; }
        public Stock stock { get; set; }

        public string Narration { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q9 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q8 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q7 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q6 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q5 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q4 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q3 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q2 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q1 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal Q0 { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal? observationValue { get; set; }

        public DateTime RecordTimeStamp { get; set; }
        public string observationValueType { get; set; }

    }
}