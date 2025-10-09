namespace FaysalFunds.Common
{
    public class Settings
    {
        public string DateFormat { get; set; }=string.Empty;
        public double lockMinutes { get; set; }
        public double OtpExpirationSeconds { get; set; }
        public int BasicInfoSteps { get; set; }
        public int PersonalDetailSteps { get; set; }
        public int RiskProfileSteps { get; set; }
        public int RegulatoryKycSteps { get; set; }
        public int UploadDocumentSteps { get; set; }
    }
}
