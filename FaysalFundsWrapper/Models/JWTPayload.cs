namespace FaysalFundsWrapper.Models
{
    public class JWTPayload
    {
        public long UserId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
    }
}
