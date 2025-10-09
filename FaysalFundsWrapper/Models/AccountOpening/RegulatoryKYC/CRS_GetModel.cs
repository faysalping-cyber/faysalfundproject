using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsWrapper.Models
{
    public class CRS_GetModel
    {
        public int? TaxResidentCountry { get; set; }
        public string? TIN_Number { get; set; }
        public int? HaveTIN { get; set; }
        public int? ReasonForNoTIN { get; set; }
        public int? CRS_Declaration { get; set; }
    }
}
