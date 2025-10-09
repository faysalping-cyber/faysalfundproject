using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("FAMLFUNDS")]
    public class FamlFund
    {
        public int ID { get; set; }
        public string TITLE { get; set; } = string.Empty;
        public int PERTRANSACTIONLIMIT { get; set; }
        public int ANNUALINVESTMENTLIMIT { get; set; }
        public int ALLTIMEINVESTMENTLIMIT { get; set; }
        public int STATUS { get; set; }
        public int FIRST_TRANSACTION_MIN { get; set; }
        public int SUBSEQUENT_TRANSACTION_MIN { get; set; }
    }
}