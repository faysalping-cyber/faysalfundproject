namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class StepCount
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public int? TotalSteps { get; set; }
        public int? CompletedSteps { get; set; }
        public int? Enabled { get; set; }
    }

}
