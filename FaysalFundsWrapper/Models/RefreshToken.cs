namespace FaysalFundsWrapper.Models
{
    public class RefreshToken
    {
        public string TOKEN { get; set; } = string.Empty;
        public long ACCOUNTID { get; set; }
        public DateTime EXPIRYDATE { get; set; }
        public byte ISREVOKED { get; set; } = 0;
        public string EMAIL { get; set; } = string.Empty;
    }
}
