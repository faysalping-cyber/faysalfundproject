using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening.RegulatoryKYC
{
    public class RegulatoryKYC_Step2GetModel
    {
        public int? FinantialInstitutionRefusal { get; set; }
        public int? UltimateBeneficiary { get; set; }
        public int? HoldingSeniorPosition { get; set; }
        public int? InternationalRelationshipOrPREP { get; set; }
        public int? DealingWithHighValueItems { get; set; }
        public int? FinancialLinksWithOffshoreTaxHavens { get; set; }
        public int? TrueInformationDeclaration { get; set; }
    }
}
