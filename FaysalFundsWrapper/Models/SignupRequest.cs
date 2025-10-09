namespace FaysalFundsWrapper.Models
{
    public class SignupRequest
    {
        public string? CountryCode { get; set; } = string.Empty;
        public string? PhoneNo { get; set; } = string.Empty;
        public string? CNIC { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        //public bool? IsActiveOnWhatsApp { get; set; } 
    }
}
