using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class CalculateReceiptDTO
    {
        public int FolioNumber { get; set; }
        public string FundFrom { get; set; } = string.Empty;
        public string FundTo { get; set; } = string.Empty;
        public decimal AmountConverted { get; set; }
        public decimal FELCharges { get; set; }
        public string CGTApplicable { get; set; }
        public decimal TotalAmount { get; set; }
        public int MonthlyProfit { get; set; }
        public decimal AvailableBalanceAtTransaction { get; set; }

    }
    public class CaculateReceiptPayload
    {
        public int UserId { get; set; }
        public int FolioNumber { get; set; }
        public int OldFundId { get; set; }
        public int NewFundId { get; set; }
        public decimal ConversionAmount { get; set; }
        public long PAYMENTMODE { get; set; }
        public bool CheckConvertAll { get; set; }
    }
}
