using FaysalFunds.Domain.Entities;
using FaysalFunds.Common;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using System;

namespace FaysalFunds.Domain.Interfaces
{
    public interface IQuickAccessRepository
    {
        Task<bool> AddQuickAccessMenu(QuickAccess menu);
        Task<List<QuickAccess>> GetAllQuickAccessMenu();
    }
}
