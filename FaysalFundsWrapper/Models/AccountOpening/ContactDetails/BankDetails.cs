namespace FaysalFundsWrapper.Models
{
    public class BankDetails
    {
        public long UserId { get; set; }
        public string TypeOfAccount { get; set; } = string.Empty;
        public int? Bank { get; set; }
        public int? Wallet { get; set; }
        public string IBAN { get; set; } = string.Empty;
        public string WalletNumber { get; set; } = string.Empty;
        public string DividendPayout { get; set; } = string.Empty;
    }
}
