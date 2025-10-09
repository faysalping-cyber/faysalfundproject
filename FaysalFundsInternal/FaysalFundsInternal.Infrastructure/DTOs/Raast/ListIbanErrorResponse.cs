using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsInternal.Infrastructure.DTOs.Raast
{
    public class ListIbanErrorResponse
    {
        public string ResponseCode { get; set; }  = string.Empty;
        public string ResponseMessage { get; set; } = string.Empty;
    }
}
