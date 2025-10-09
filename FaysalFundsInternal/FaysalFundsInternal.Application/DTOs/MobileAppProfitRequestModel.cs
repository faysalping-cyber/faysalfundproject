using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsInternal.Application.DTOs
{
    public class MobileAppProfitRequestModel
    {
        public string CNIC { get; set; }
        public string PhoneNo { get; set; }
        public long? FolioNo { get; set; }
    }
}
