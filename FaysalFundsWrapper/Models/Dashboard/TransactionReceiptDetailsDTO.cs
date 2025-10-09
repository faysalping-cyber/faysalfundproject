namespace FaysalFundsWrapper.Models.Dashboard
{
    public class TransactionReceiptDetailsDTO
    {
        public string Id { get; set; }

        public DateTime DateTime { get; set; }

        public int FolioNumber { get; set; }

        public string TransactionType { get; set; }

        public string PaymentMode { get; set; }

        public string FundName { get; set; }

        public int FelCharges { get; set; }

        public int KuickPayCharges { get; set; }

        public int AmountInvested { get; set; }
        public string MonthlyProfit { get; set; }

        public decimal TotalAmount { get; set; }

        public string KuickPayId { get; set; }
        public string? Iban { get; set; }
        public string? BankName { get; set; }
        //public string? AccountTitle { get; set; }
        public int? IsExistingAccount { get; set; }
        public int? ACKNOWLEDGE { get; set; }
        public byte[]? TransactionProofPath { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class TransactionReceiptPayload
    {
        public int FolioNumber { get; set; }
        public long FundID { get; set; }
        public int Invested { get; set; }
        //public long AccountTypeID { get; set; }
        public long UserId { get; set; }

        //public int KPCharges { get; set; }
        //public int TotalAmpunt { get; set; }
        //public int Feldeducation { get; set; }
        public int MonthlyProfit { get; set; }
        //public int AmountInvested { get; set; }
        public string kuickPayID { get; set; }
        //public string TransactionTyp { get; set; }
        public long PaymentMode { get; set; }
        public string BankName { get; set; }
        public string AccountTitle { get; set; }
        public string IBAN { get; set; }
        public byte[]? TransactionProof { get; set; }
        public int IsExistingBank { get; set; }
        public int? ACKNOWLEDGE { get; set; }

    }

}
