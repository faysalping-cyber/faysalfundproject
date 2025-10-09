using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("REFRESHTOKEN")]
    public class RefreshToken
    {
        public int ID { get; set; }
        public string TOKEN { get; set; } = string.Empty;
        public long ACCOUNTID { get; set; } 
        public string EMAIL { get; set; } = string.Empty;
        public DateTime EXPIRYDATE { get; set; }

        public byte ISREVOKED { get; set; } = 0;

        // Navigation property (assuming Identity)
        public Account ACCOUNT { get; set; } = null!;
    }

}
