using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class FundTransactionGroupDTO
    {
      
        public long FundId { get; set; }
        public long TransactionTypeId { get; set; }

    }
    public class FeaturePermissionResultDTO
    {
        public string FeatureName { get; set; }
        public string FeatureGroup { get; set; }
        public bool IsAllowed { get; set; }
        
    }
    public class FeaturePermissionRequestDTO
    {
        public long FundId { get; set; }
        public long TransactionFeatureId { get; set; }
    }

}
