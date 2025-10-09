using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsInternal.Application.DTOs
{
    public class GenerateKuickPayIdDTO
    {
        public string FolioNo { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
    public class KuickPayResponseModel
    {
        public string response_Code { get; set; }
        public string response_description { get; set; }
        public string responseCode_Detail { get; set; }
    }

    public class KuickPayRequestModel
    {
        public string institutionID { get; set; } = "01520";
        public string registrationNumber { get; set; } = string.Empty;
        public string Head1 { get; set; } = string.Empty;
        public decimal Amount1 { get; set; }
        public string Head2 { get; set; } = string.Empty;
        public decimal Amount2 { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal AmountAfterDueDate { get; set; }

        public string DueDate { get; set; }
        public string ExpiryDate { get; set; }
        public string IssueDate { get; set; }
        public string VoucherMonth { get; set; }
        public string VoucherYear { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
    }
}
