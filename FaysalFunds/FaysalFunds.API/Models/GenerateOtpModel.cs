namespace FaysalFunds.API.Models
{
    public class GenerateOtpModel
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CountryCode { get; set; }
        public bool IsWhatsapp { get; set; }
        public string OtpToken { get; set; } = string.Empty;
        public bool SameOtp { get; set; } = false;
    }
}
