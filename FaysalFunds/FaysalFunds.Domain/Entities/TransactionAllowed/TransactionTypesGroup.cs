using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.TransactionAllowed
{

    [Table("TRANSACTION_TYPES_GROUPS")] //this table save the name on lye like investment conversion and withdrawles
    public class TransactionTypesGroup
    {
        public long ID { get; set; }

        public string GROUP_NAME { get; set; }
    }
}
