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
 

    [Table("QUICK_ACCESS")]
    public class QuickAccess
    {
        public long ID { get; set; }

        public string NAME { get; set; }

        public byte[]? ICON { get; set; } // Base64 or URL

        public int ACTIVE { get; set; }

        //[ForeignKey(nameof(TransactionFeatures))]
        //public long? TRANSACTION_FEATURE_ID { get; set; }

    }





}
