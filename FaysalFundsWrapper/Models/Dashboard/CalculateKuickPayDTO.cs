namespace FaysalFundsWrapper.Models.Dashboard
{
   
    public class CalculateKuickPayDTO
    {
        public string FundName { get; set; }
        public int FolioNumber { get; set; }
        public int Invested { get; set; }
        public int TotalAmount { get; set; }
        public string FelCharges { get; set; }
        public int AmountInvested { get; set; }
        public string MonthlyProfit { get; set; }
        public string? KPCharges { get; set; }   // ✅ only KP has this required
    }

    public class CalculateKuickPayLoad
    {
        public long FundID { get; set; }
        public int FolioNumber { get; set; }
        public int Invested { get; set; }
        public long UserId { get; set; }
        public long PaymentMode { get; set; }

    }
}
