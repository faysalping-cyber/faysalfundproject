using FaysalFundsInternal.Domain.Entities;
using FaysalFundsInternal.Domain.Interfaces;
using FaysalFundsInternal.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FaysalFundsInternal.Persistence.Repositories
{
    public class UnitHolderRepository : BaseRepository<UnitHolder>, IUnitHolderRepository
    {
        private readonly DbSet<UnitHolder> _unitHolderSet;
        public UnitHolderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _unitHolderSet = dbContext.Set<UnitHolder>();
        }
        public async Task<string> GetUnitHolderId(long folioNo)
        {
            string foliostr = folioNo.ToString();
            var unitHolderId = await _unitHolderSet
                    .Where(unitholder => unitholder.REGISTRATION_NO_2 == foliostr)
                    .Select(unitholder => unitholder.UNIT_HOLDER_ID)
                    .FirstOrDefaultAsync();
            return unitHolderId;
        }
    }
}
