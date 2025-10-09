using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IJWTBlackListRepository
    {
        Task<int> BlackListToken(string token, DateTime expirationDate);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
