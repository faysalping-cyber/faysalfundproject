using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("LOGIN_ATTEMPTS")]
    public class LoginAttempt
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public long ACCOUNTID { get; set; }

        public int FAILED_ATTEMPTS { get; set; } = 0;

        public DateTime? LAST_ATTEMPT { get; set; }

        public int IS_LOCKED { get; set; } = 0;

        public DateTime? LOCK_UNTIL { get; set; }
    }
}
