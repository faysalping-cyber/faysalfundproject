using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening.BasicInformation
{
    public class BasicInformation1GetModel
    {
        public string? FullName { get; set; }
        public string? FatherOrHusbandName { get; set; }
        public string? MotherName { get; set; }
        public string? CNIC { get; set; }
        public string? CNIC_IssueDate { get; set; }
        public string? CNIC_ExpiryDate { get; set; }
        public int IsCnicExpiryLifetime { get; set; }
    }
}
