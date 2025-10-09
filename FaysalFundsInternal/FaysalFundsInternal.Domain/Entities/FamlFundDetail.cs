using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFundsInternal.Domain.Entities
{
    [Table("FAML_FUND_DETAIL")]
    [Keyless]
    public class FamlFundDetail
    {
        [Key]
        public string FUND_ID { get; set; }
        public DateTime APPLICABLE_NAV_DATE { get; set; }
        public decimal NAV_PER_UNIT { get; set; }
    }
}
