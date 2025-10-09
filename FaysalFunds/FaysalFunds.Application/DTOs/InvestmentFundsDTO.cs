using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class InvestmentFundsDTO
    {

        public List<FundItem> Low { get; set; }
        public List<FundItem> Medium { get; set; }
        public List<FundItem> High { get; set; }
    }
 
    public class FundItem
    {
        public long ID { get; set; }
        public string FUNDNAME { get; set; }
        public string FUNDCATEGORY { get; set; }
        public ViewDetails VIEWDETAIL { get; set; } // 🆕 Add this


    }
    public class ViewDetails
    {
        public string RISKPROFILE { get; set; }
        public string GENDER { get; set; }
        public string MONTHLYPROFILT { get; set; }
        public string FELPERCENTAGE { get; set; }
        public string ISENABLE { get; set; }
        public DateTime CREATEDON { get; set; }
    }
}
