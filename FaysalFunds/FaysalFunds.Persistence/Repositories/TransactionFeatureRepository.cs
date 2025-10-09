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
    public class TransactionFeatureRepository : BaseRepository<TransactionFeatures>, ITransactionFeatureRepository
    {
        private readonly DbSet<TransactionFeatures> _transactionFeature;
        public TransactionFeatureRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _transactionFeature = dbContext.Set<TransactionFeatures>();
        }

        public async Task<List<TransactionFeatures>> GetAllFeatures()
        {
            return await _transactionFeature.ToListAsync();
        }

        public async Task<TransactionFeatures> GetTransactionFeatureById(long TransactionFeatureID)
        {
            return await _transactionFeature
              .Where(f => f.ID == TransactionFeatureID)
                .FirstOrDefaultAsync();
        }
    }
}
