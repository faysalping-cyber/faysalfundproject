namespace FaysalFundsWrapper.Models
{
    public class JwtResponse
    {
        public bool IsDeviceRegistered { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
