namespace FaysalFundsWrapper.Models
{
    public class LoginResponseModel
    {
        public long UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cnic { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public bool IsDeviceRegistered { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool TpinExist { get; set; }

    }
}
