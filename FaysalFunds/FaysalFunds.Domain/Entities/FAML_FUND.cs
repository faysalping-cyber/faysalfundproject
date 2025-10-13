using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("FAML_FUND")]
    public class FAML_FUND
    {
        public string FUND_SHORT_NAME { get; set; } = string.Empty;
        public string FUND_NAME { get; set; } = string.Empty;
    }
}
