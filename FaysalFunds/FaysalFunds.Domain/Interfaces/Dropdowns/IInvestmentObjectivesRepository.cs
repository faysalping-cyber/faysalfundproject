using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces.Dropdowns
{
    public interface IInvestmentObjectivesRepository
    {
        Task<IEnumerable<InvestmentObjective>> GetAll();
    }
}
