using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("OTP_VERIFICATION")]
    public class Otp
    {

        public long ID { get; set; }
        public long USER_ID { get; set; }
        public string EMAIL_OTP { get; set; }
        public string MOBILE_OTP { get; set; }
        public byte IS_EMAIL_VERIFIED { get; set; }
        public byte IS_MOBILE_VERIFIED { get; set; }
        public byte IS_WHATSAPP { get; set; }
        public string OTP_TOKEN { get; set; }
        public int ATTEMPT_COUNT { get; set; }
        public DateTime CREATED_AT { get; set; }
        public DateTime EXPIRES_AT { get; set; }
    }
}


