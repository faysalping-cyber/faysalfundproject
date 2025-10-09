using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities
{
    [Table("USERPASSWORDHISTORY")]
    public class UserPasswordHistory
    {
        [Key]
        public int ID { get; set; }

        // Renamed UserId to AccountId for better FK convention
        [ForeignKey("ACCOUNTID")]
        public long ACCOUNTID { get; set; }

        public string HASHEDPASSWORD { get; set; } = string.Empty;
        public DateTime CREATEDON { get; set; }

        // Nullable navigation property to prevent issues
        public Account? ACCOUNT
        {
            get; set;
        }
    }
}