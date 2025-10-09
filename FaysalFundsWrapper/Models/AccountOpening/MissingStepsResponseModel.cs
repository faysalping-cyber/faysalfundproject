namespace FaysalFundsWrapper.Models.AccountOpening
{
    public class MissingStepsResponseModel
    {
        public string ProfileName { get; set; }
        public string Message { get; set; }
        public string OnboardingStatusMessage { get; set; }
        public string Status { get; set; }

        public List<string> MissingSteps { get; set; }
    }
}
