namespace FaysalFundsWrapper.Models
{
    public class RefreshTokenResponseModel
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public byte IsRevoked { get; set; } = 0;
    }
}
