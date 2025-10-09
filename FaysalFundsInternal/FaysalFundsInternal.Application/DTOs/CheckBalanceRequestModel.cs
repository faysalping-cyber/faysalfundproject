using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsInternal.Application.DTOs
{
    public class CheckBalanceRequestModel
    {
        public long? Folio { get; set; }
        public string Cnic { get; set; }
        public string PhoneNo { get; set; }
    }
}
