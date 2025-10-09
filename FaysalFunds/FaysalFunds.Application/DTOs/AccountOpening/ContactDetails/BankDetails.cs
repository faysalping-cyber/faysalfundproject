namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class BankDetails
    {
        //public long UserId { get; set; }
        //public int Bank { get; set; }
        //public int BranchCity { get; set; }
        //public string BranchName { get; set; } = string.Empty;
        //public string AccountNumber { get; set; } = string.Empty;
        //public string IBAN { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string TypeOfAccount { get; set; } = string.Empty;
        public int? Bank { get; set; }
        public int? Wallet { get; set; }
        public string IBAN { get; set; } = string.Empty;
        public string WalletNumber { get; set; } = string.Empty;
        public string DividendPayout { get; set; } = string.Empty;
    }
}
