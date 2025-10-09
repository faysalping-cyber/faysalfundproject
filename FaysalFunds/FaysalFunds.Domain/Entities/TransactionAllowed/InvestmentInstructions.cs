using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Entities.TransactionAllowed
{
    [Table("INVESTMENT_INSTRUCTIONS")]

    public class InvestmentInstructions
    {
        public long ID { get; set; }
        public string CHANNEL { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
    }
}
