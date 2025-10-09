using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class SignupResponseModel
    {
        public long UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
