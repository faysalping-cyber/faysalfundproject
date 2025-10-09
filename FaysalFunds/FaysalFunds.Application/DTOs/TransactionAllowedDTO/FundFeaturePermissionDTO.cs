using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.TransactionAllowedDTO
{
    public class FundFeaturePermissionDTO
    {
        public long ID { get; set; }
        public int FundId { get; set; }
        public int TransactionFeatureId { get; set; }
        public string IsAllowed { get; set; }
    }
    public class FeaturePermissionResponse
    {
        public bool IsAllowed { get; set; }
    }
}
