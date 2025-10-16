using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class AlreadyInvestedFundsDTO
    {
        public long FundID { get; set; }
        public string FundName { get; set; }
        public string MonthlyProfit { get; set; }
        //public int FundAmount { get; set; }
        public string FundCategory { get; set; }
        public decimal TotalAmount { get; set; }
        public string RiskProfile { get; set; }

    }
}
