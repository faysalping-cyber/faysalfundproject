using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs
{
    public class RefreshTokenResponse
    {
        
        
            public int Id { get; set; }
            public string Token { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public DateTime ExpiryDate { get; set; }
            public byte IsRevoked { get; set; } = 0;
        
    }
}
