using FaysalFunds.Domain.Entities.TransactionAllowed;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Persistence.Repositories
{
    public class FundTransactionGroupRepository : BaseRepository<FundTransactionGroup>, IFundTransactionGroupRepository
    {
        private readonly DbSet<FundTransactionGroup> _fundTransactionGroup;
        public FundTransactionGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _fundTransactionGroup = dbContext.Set<FundTransactionGroup>();
        }


        //public async Task<bool> IsFundInTransactionGroup(long fundId, long transactionTypeId)
        //{
        //    return await _fundTransactionGroup
        //        .AnyAsync(x => x.FUND_ID == fundId && x.TRANSACTION_TYPE_ID == transactionTypeId);
        //}

        public async Task<List<FundTransactionGroup>> IsFundInTransactionGroup(long fundId, long transactionTypeId)
        {
            return await _fundTransactionGroup
                .Where(x => x.FUND_ID == fundId && x.TRANSACTION_TYPE_ID == transactionTypeId)
                .ToListAsync();
        }

    }
}

