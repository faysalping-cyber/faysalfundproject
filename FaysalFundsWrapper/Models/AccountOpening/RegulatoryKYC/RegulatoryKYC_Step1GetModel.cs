using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsWrapper.Models
{
    public class RegulatoryKYC_Step1GetModel
    {
        public int? Occupation { get; set; }
        public int? Profession { get; set; }
        public int? SourceOfIncome { get; set; }
        public string? NameOfEmployerOrBussiness { get; set; }
        public decimal? GrossAnnualIncome { get; set; }
        public int? MonthlyExpectedInvestmentAmount { get; set; }
        public int? MonthlyExpectedNoOfInvestmentTransaction { get; set; }
        public int? MonthlyExpectedNoOfRedemptionTransaction { get; set; }
    }
}
