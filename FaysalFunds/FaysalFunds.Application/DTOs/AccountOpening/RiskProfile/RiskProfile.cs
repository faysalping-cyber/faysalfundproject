namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class RiskProfile
    {
        public long UserId { get; set; }
        public int? Age { get; set; }
        public int? MaritalStatus { get; set; }
        public int? NoOfDependents { get; set; }
        public int? Occupation { get; set; }
        public int? Education { get; set; }
        public int? RiskAppetite { get; set; }
        public int? InvestmentObjective { get; set; }
        public int? InvestmentHorizon { get; set; }
        public int? InvestmentKnowledge { get; set; }
        public int? FinancialPosition { get; set; }
        public int RiskProfileDeclaration { get; set; }

    }
}
