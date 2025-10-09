namespace FaysalFundsInternal.Application.DTOs
{
    public class CheckBalanceModel
    {
        public long FolioNo { get; set; }
        public string FundName { get; set; }
        public string AccountType { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
