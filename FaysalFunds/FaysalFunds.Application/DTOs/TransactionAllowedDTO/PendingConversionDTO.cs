using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class PendingConversionDTO
    {
        public int STATUS { get; set; }
        public long OLD_FUND_ID { get; set; }
        public int FOLIONUMBER { get; set; }

    }
}
