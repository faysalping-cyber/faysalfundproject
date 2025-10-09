namespace FaysalFunds.Application.DTOs
{
    public class Login
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RegisteredDeviceId { get; set; } = string.Empty;
    }
}
