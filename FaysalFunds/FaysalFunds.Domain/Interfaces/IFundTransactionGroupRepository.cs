using FaysalFunds.Common;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IFundTransactionGroupRepository
    {
        //Task<List<FundTransactionGroup>> GetFundTransactionGroup();
        Task<List<FundTransactionGroup>> IsFundInTransactionGroup(long fundId, long transactionTypeId);
        
    }
}
