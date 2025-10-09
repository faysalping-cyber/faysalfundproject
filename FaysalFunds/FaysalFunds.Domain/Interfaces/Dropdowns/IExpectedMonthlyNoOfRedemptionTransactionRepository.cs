using FaysalFunds.Domain.Entities.Dropdowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IExpectedMonthlyNoOfRedemptionTransactionRepository
    {
        Task<IEnumerable<ExpectedMonthlyNoOfRedemptionTransaction>> GetAll();

    }
}
