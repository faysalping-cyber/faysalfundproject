namespace FaysalFunds.Application.DTOs
{
    public class Fund
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PerTransactionLimit { get; set; }
        public int AnnualInvestmentLimit { get; set; }
        public int AllTimeInvestmentLimit { get; set; }
        public int FirstTransactionMin { get; set; }
        public int SubsequentTransactionMin { get; set; }
    }
}
