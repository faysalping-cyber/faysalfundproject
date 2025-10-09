using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class JWTPayload
    {
        public string UserId { get; set; }

        public string Email { get; set; }
    }
}
