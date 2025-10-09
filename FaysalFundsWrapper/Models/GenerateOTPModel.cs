namespace FaysalFundsWrapper.Models
{
    public class GenerateOTPModel
    {
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? CountryCode { get; set; }
        public bool? IsWhatsapp { get; set; }
        public bool? SameOtp { get; set; } = false;
        public string? OtpToken { get; set; }

    }
}