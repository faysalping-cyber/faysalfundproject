using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class RedemptionReceiptDetailDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public int FolioNumber { get; set; }
        public string FundName { get; set; } = string.Empty;
        public decimal RedemptionAmount { get; set; }
        public decimal AvailableBalanceAtTransaction { get; set; }
        public string Status { get; set; } = "Pending";
        public long PAYMENTMODE { get; set; }

        public DateTime CreatedOn { get; set; }
    }
    public class RedemptionReceiptPayload
    {
        public int UserId { get; set; }
        public int FolioNumber { get; set; }
        public int FundId { get; set; }
        public decimal RedemptionAmount { get; set; }
        public string Pin { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public long PAYMENTMODE { get; set; }

    }

}
