using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.TransactionAllowed
{

    [Table("TRANSACTION_FEATURES")]  //this table save the name of transaction
    public class TransactionFeatures
    {
        public long ID { get; set; }

        public string FEATURE_NAME { get; set; }

        public string FEATURE_GROUP { get; set; }
        public string PAYMENT_MODE { get; set; }
        public byte[]? ICON { get; set; } // Base64 or URL

    }
}
