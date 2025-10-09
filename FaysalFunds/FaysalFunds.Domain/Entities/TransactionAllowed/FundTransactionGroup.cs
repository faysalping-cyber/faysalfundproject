using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.TransactionAllowed
{
    [Table("FUND_TRANSACTION_GROUPS")] 
    public class FundTransactionGroup
    {
        public long ID { get; set; }

        [ForeignKey(nameof(Fund))]
        public long FUND_ID { get; set; }

        [ForeignKey(nameof(TransactionGroup))]
        public long TRANSACTION_TYPE_ID { get; set; }

        public virtual InvestmentFunds Fund { get; set; }
        public virtual TransactionTypesGroup TransactionGroup { get; set; }
    }
}
