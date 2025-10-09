using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFundsInternal.Domain.Entities
{
    [Table("FAML_FUND")]
    [Keyless]
    public class FamlFund
    {
        public string FUND_ID { get; set; }
        public string FUND_NAME { get; set; }
    }
}
