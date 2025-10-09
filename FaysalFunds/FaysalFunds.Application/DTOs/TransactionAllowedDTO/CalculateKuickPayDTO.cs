using FaysalFunds.Domain.Entities;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{

  
    public class CalculateKuickPayDTO 
    {
        public string FundName { get; set; }
        public int FolioNumber { get; set; }
        public int Invested { get; set; }
        public int TotalAmount { get; set; }
        public string FelCharges { get; set; }
        public int AmountInvested { get; set; }
        public string MonthlyProfit { get; set; }
        public string? KPCharges { get; set; }   // ✅ only KP has this required
    }
 
    public class CalculateKuickPayLoad
    {
        public long FundID { get; set; }
        public int FolioNumber { get; set; }
        public int Invested { get; set; }
        public long UserId { get; set; }
        public long PaymentMode { get; set; }
 
    }



}
