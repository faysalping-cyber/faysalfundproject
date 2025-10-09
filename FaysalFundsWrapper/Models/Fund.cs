namespace FaysalFundsWrapper.Models
{
    public class Fund
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PerTransactionLimit { get; set; }
        public int AnnualInvestmentLimit { get; set; }
        public int AllTimeInvestmentLimit { get; set; }
    }
}
