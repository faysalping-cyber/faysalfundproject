namespace FaysalFundsWrapper.Models
{
    public class ForgotPasswordModel
    {
        public string? Email { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        public string? NewPassword { get; set; } = string.Empty;
    }
    public class ChangePasswordModel
    {
        public long? UserId { get; set; }
        public string? OldPassword { get; set; } = string.Empty;
        public string? NewPassword { get; set; } = string.Empty;
    }
}
