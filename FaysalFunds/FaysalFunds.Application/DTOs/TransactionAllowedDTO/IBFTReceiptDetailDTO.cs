using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class IBFTReceiptDetailDTO
    {
        public string TransactionID { get; set; }

        public DateTime DateTime { get; set; }

        public int FolioNumber { get; set; }

        public string TransactionType { get; set; }

        public string PaymentMode { get; set; }

        public string FundName { get; set; }

        public int FelCharges { get; set; }

        public int AmountInvested { get; set; }
        public string MonthlyProfit { get; set; }

        public decimal TotalAmount { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        //public string? AccountTitle { get; set; }
        public int? IsExistingAccount { get; set; }
        public int ACKNOWLEDGE { get; set; }
        public byte[] TransactionProofPath { get; set; }
        public DateTime CreatedOn { get; set; }
    }


    public class IBFTReceiptPayload
    {
        public int FolioNumber { get; set; }
        public long FundID { get; set; }
        public int Invested { get; set; }
        public long UserId { get; set; }


        public int MonthlyProfit { get; set; }
        public long PaymentMode { get; set; }
        public string BankName { get; set; }
        public string IBAN { get; set; }
        public byte[] TransactionProof { get; set; }
        public int IsExistingBank { get; set; }
        public int ACKNOWLEDGE { get; set; }
        public string Pin { get; set; }
    }

}
