using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFundsInternal.Domain.Entities
{
    [Table("FAML_UM_ACCOUNT_STATEMENT")]
    public class FamlAccountSatement
    {
        [Key]
        public long RECORD_ID { get; set; }
        public string UNIT_HOLDER_ID { get; set; }
        public decimal GROSS_AMOUNT { get; set; }
        public decimal NO_OF_UNITS { get; set; }
        public string STAMPING_FIELD { get; set; }
        public string STATUS { get; set; }
        public string FUND_ID { get; set; }
        public DateTime NAV_DATE { get; set; }
    }
}
