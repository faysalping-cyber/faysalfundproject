using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening.RegulatoryKYC
{
    public class FATCA
    {
        public long UserId { get; set; }
        public string CountryOfTaxResidenceElsePakistan { get; set; }
        public int IsUS_CitizenResidentOrHaveGreenCard { get; set; }
        public int HaveUS_TelephoneNumber { get; set; }
        public string US_TaxPayerIdentificationNumber { get; set; }
        public int InstructionsToTransferFundsToUSA { get; set; }
        public int HaveUS_ResidenceMailingOrHoldingAddress { get; set; }
        public int IsUs_Born { get; set; }
        public int FATCA_Declaration { get; set; }
        public int NonUS_PersonDeclaration { get; set; }
        public byte[] W9FormUpload { get; set; }
    }
}
