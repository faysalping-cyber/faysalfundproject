using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class UserQuickAccessDto
    {
        public long USERID { get; set; }
        public long QUICKACCESSID { get; set; }
        //public long TRANSACTIONFEATUREID { get; set; }
    }

    public class RemoveUserQuickAccessDto
    {
        public long USERID { get; set; }
        public long QUICKACCESSID { get; set; }
    }
}
