using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class ConversionReceiptDetailDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public int FolioNumber { get; set; }
        public string FundFrom { get; set; } = string.Empty;  // 🟢 Old Fund Name
        public string FundTo { get; set; } = string.Empty;    // 🟢 New Fund Name
        public decimal AmountConverted { get; set; }
        public decimal FELCharges { get; set; }
        public decimal MonthlyProfit { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class ConversionReceiptPayload
    {
        public int UserId { get; set; }
        public int FolioNumber { get; set; }
        public int OldFundId { get; set; }
        public int NewFundId { get; set; }
        public decimal ConversionAmount { get; set; }
        //public string Pin { get; set; }
        public bool CheckConvertAll { get; set; }
        public long PAYMENTMODE { get; set; }
        public int? ACKNOWLEDGE { get; set; }


    }

}
