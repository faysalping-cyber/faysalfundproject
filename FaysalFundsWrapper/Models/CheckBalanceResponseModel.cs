namespace FaysalFundsWrapper.Models
{
    public class CheckBalanceResponseModel
    {
        public long FolioNo { get; set; }
        public string FundName { get; set; }
        public string AccountType { get; set; }
        public decimal BalanceAmount { get; set; }
    }
    public class CheckBalance
    {
        public decimal TotalBalance { get; set; }
        public List<CheckBalanceResponseModel> CheckBalanceList { get; set; }
    }
}
