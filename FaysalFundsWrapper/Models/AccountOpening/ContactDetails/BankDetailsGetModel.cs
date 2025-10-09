namespace FaysalFundsWrapper.Models
{
    public class BankDetailsGetModel
    {
        public string? TypeOfAccount { get; set; }
        public int? Bank { get; set; }
        public int? Wallet { get; set; }
        public string? IBAN { get; set; }
        public string? WalletNumber { get; set; }
        public string? DividendPayout { get; set; }
    }
}
