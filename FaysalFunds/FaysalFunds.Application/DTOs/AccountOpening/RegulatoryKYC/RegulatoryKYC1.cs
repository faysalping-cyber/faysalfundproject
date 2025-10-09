namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class RegulatoryKYC1
    {
        public long UserId { get; set; }
        public int Occupation { get; set; }
        public int Profession { get; set; }
        public int SourceOfIncome { get; set; }
        public string NameOfEmployerOrBussiness { get; set; }
        public decimal GrossAnnualIncome { get; set; }
        public int MonthlyExpectedInvestmentAmount { get; set; }
        public int MonthlyExpectedNoOfInvestmentTransaction { get; set; }
        public int MonthlyExpectedNoOfRedemptionTransaction { get; set; }
    }
}