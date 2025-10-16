using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class KuickPayReceiptDetailsDTO
    {
        public string TransactionID { get; set; }

        public DateTime DateTime { get; set; }

        public int FolioNumber { get; set; }

        public string TransactionType { get; set; }

        public string PaymentMode { get; set; }

        public string FundName { get; set; }

        public int FelCharges { get; set; }

        public int? KuickPayCharges { get; set; }

        public int AmountInvested { get; set; }
        public string MonthlyProfit { get; set; }

        public decimal TotalAmount { get; set; }

        public string KuickPayId { get; set; }
       public int ACKNOWLEDGE { get; set; }
        public DateTime CreatedOn { get; set; }   
    }


    public class KuickPayReceiptPayload
    {
        public int FolioNumber { get; set; }
        public long FundID { get; set; }
        public int Invested { get; set; }
        public long UserId { get; set; }

   
        public int MonthlyProfit { get; set; }
        public string kuickPayID { get; set; }
        public long PaymentMode { get; set; }
        public int ACKNOWLEDGE { get; set; }
        public string Pin { get; set; }
    }

}
