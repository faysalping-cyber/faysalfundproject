using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.ExternalAPI
{
    public class RaastIdsListResponseModel
    {
        public string FundName { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
    }
}
