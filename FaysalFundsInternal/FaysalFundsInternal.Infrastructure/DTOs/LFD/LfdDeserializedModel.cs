using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsInternal.Infrastructure.DTOs.LFD
{
    public class LfdDeserializedModel
    {
        public string error { get; set; } = string.Empty;
        public string error_code { get; set; } = string.Empty;
        public string matched { get; set; } = string.Empty;
        public string flag { get; set; } = string.Empty;
    }
}
