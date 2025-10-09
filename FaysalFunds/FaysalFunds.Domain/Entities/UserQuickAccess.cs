using FaysalFunds.Domain.Entities.TransactionAllowed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities
{
    [Table("USER_QUICK_ACCESS")]
    public class UserQuickAccess
    {
        public long ID { get; set; }

        public long USERID { get; set; }

        [ForeignKey(nameof(QuickAccess))]
        public long QUICKACCESSID { get; set; }

        //[ForeignKey(nameof(TransactionFeatures))]
        //public long? TRANSACTION_FEATURE_ID { get; set; }


    }
}
