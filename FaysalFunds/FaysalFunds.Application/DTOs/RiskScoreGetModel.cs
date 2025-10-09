namespace FaysalFunds.Application.DTOs
{
    public class RiskScoreGetModel
    {
        public int Score { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
    }
}