using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.TransactionAllowed
{
    
     [Table("KP_SLAB")]
    [Keyless]
    public class kpSlab
    {
        public string LOWER_LIMIT { get; set; } // VARCHAR2

        public string UPPER_LIMIT { get; set; } // VARCHAR2, nullable for NULL values

        public decimal FEE_LIMIT { get; set; } // NUMBER
    }
}
