namespace FaysalFundsWrapper.Models
{
    public class LogoutRequestModel
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
