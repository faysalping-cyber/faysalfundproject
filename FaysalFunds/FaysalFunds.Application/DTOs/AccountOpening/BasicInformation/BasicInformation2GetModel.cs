using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening.BasicInformation
{
    public class BasicInformation2GetModel
    {
        public string? DOB { get; set; }
        public int? PlaceOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ResidentialStatus { get; set; }
        public string? ZakatStatus { get; set; }
        public byte[]? UploadCZ_50 { get; set; }
        public string? ReferralCode { get; set; }
    }
}
