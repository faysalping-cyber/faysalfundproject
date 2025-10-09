using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int EmailVerified { get; set; }
    }
}
