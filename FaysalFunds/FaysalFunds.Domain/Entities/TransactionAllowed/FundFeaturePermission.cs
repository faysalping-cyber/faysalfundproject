using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.TransactionAllowed
{
    [Table("FUND_FEATURE_PERMISSIONS")]
    public class FundFeaturePermission
    {
        public long ID { get; set; }
        public long FUND_ID { get; set; }
        public long TRANSACTION_FEATURE_ID { get; set; }
        public string IS_ALLOWED { get; set; }
    }
}
