using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class VerifyUserBeforeForgotTpin
    {
        public long UserId { get; set; }
        public string Email { get; set; }    
        public string Cnic { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
              
    }
}
