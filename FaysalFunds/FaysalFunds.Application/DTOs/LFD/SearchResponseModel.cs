using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class SearchResponseModel
    {
        public string error { get; set; }
        public string error_code { get; set; }
        public string matched { get; set; }
        public string flag { get; set; }
        //public DataModel Data { get; set; } // Assuming `data` is an object, not a string
        public string hmac_signature { get; set; }
    }
}
