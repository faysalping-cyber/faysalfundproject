namespace FaysalFunds.Application.DTOs.OTP
{
    public class GenerateOtpResponseModel
    {
        public string OtpToken { get; set; }=string.Empty;
        public int OtpExpiry{ get; set; }
    }
}
