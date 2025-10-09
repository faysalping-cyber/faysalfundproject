using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class SearchModel
    {
        public string uuid { get; set; }
        public string cnic { get; set; }
        public string Passport { get; set; }
        public string Name { get; set; }
        public string Date_Of_Birth { get; set; }
        public string type { get; set; }
        public string tier { get; set; }
    }
}
