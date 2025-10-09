using FaysalFunds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface InvesmentFundRepository
    {
        Task<List<InvestmentFunds>> GetAllFunds();
        Task<InvestmentFunds> GetByIdAsync(long fundId);

    }
}
