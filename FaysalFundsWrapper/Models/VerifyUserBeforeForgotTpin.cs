namespace FaysalFundsWrapper.Models
{
    public class VerifyUserBeforeForgotTpin
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Cnic { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
