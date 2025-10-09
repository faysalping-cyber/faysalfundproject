using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFundsInternal.Domain.Entities
{
    [Keyless]
    public class UHR
    {
        public long FOLIONO { get; set; }
        public string FUND_NAME { get; set; } = string.Empty;
        public string FUND_ID { get; set; } = string.Empty;
        public string NAV_DATE { get; set; } = string.Empty;
        public decimal NAV_PER_UNIT { get; set; }
        public decimal BALANCE_AMOUNT { get; set; }
        public decimal BALANCE_UNITS { get; set; }
        [NotMapped]
        public string? ACCOUNTTYPE { get; set; }
    }
}