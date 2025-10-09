using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFundsWrapper.Models
{
    public class AccountSelectionRequestModel
    {
        public long? UserId { get; set; }
        public int? AccountType { get; set; }
    }
}
