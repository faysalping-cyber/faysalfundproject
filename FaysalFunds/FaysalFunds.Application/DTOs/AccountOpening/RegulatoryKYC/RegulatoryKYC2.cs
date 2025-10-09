using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class RegulatoryKYC2
    {
        public long UserId { get; set; }
        public int FinantialInstitutionRefusal { get; set; }
        public int UltimateBeneficiary { get; set; }
        public int HoldingSeniorPosition { get; set; }
        public int InternationalRelationshipOrPREP { get; set; }
        public int DealingWithHighValueItems { get; set; }
        public int FinancialLinksWithOffshoreTaxHavens { get; set; }
        public int TrueInformationDeclaration { get; set; }
    }
}
