using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class KpSlabDTO
    {
        public string LOWER_LIMIT { get; set; } // VARCHAR2

        public string UPPER_LIMIT { get; set; } // VARCHAR2, nullable for NULL values

        public decimal FEE_LIMIT { get; set; } // NUMBER
    }
}
