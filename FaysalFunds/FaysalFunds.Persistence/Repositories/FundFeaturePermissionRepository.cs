using FaysalFunds.Domain.Entities;
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
    public class FundFeaturePermissionRepository : BaseRepository<FundFeaturePermission>, IFundFeaturePermissionRepository
    {
        private readonly DbSet<FundFeaturePermission> _fundFeaturePermission;
      
        public FundFeaturePermissionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _fundFeaturePermission = dbContext.Set<FundFeaturePermission>();
     
        }
       
        public async Task<FundFeaturePermission> IsFundInTransactionFeature(long FundId, long TransactionFeatureId)
        {
            return await _fundFeaturePermission
                .Where(f => f.FUND_ID == FundId && f.TRANSACTION_FEATURE_ID == TransactionFeatureId).FirstOrDefaultAsync();
        }
     
    }
}
