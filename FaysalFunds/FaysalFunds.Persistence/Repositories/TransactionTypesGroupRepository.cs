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
    public class TransactionTypesGroupRepository : BaseRepository<TransactionTypesGroup>, ITransactionTypesGroupRepository
    {
        private readonly DbSet<TransactionTypesGroup> _kpSlab;
        public TransactionTypesGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _kpSlab = dbContext.Set<TransactionTypesGroup>();
        }
        public async Task<List<TransactionTypesGroup>> GetTransactionTypes()
        {
            return await _kpSlab
            .ToListAsync();
        }
    }
}
