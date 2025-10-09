using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("ACCOUNTS")]
    public class Account
    {
        public long ID { get; set; }
        public string PHONE_NO { get; set; } = string.Empty;
        public string COUNTRYCODE { get; set; }
        public string CNIC { get; set; } = string.Empty;
        public string? NAME { get; set; }
        public string? EMAIL { get; set; }
        public string? PASSWORD { get; set; }
        public string? REGISTERED_DEVICE_ID { get; set; }
        public string? LFD_FLAG { get; set; }
        public int OTP_IS_VERIFIED { get; set; } = 0;
        public List<UserPasswordHistory> USERPASSWORDHISTORY { get; set; } = new();
    }
}

