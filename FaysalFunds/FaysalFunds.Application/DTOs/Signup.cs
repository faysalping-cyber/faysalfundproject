namespace FaysalFunds.Application.DTOs
{
    public class Signup
    {
        public string PhoneNo { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        //public bool OtpIsVerified { get; set; } = false;  // default false

    }
}

