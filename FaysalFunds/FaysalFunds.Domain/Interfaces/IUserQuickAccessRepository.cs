using FaysalFunds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IUserQuickAccessRepository
    {
        Task<List<long>> GetQuickAccessIdsByUserId(long userId);
        Task<long> GetCountByUserId(long userId);
        Task<UserQuickAccess?> GetExisting(long userId,long quickAccessId);
        Task<bool> Add(UserQuickAccess entity);
        Task<bool> Delete(long userId, long quickAccessId);
    }
}
