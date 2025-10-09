using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Common.Enums;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using FaysalFunds.Domain.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.Services
{
    public class DashhboardServices
    {
        //private readonly IDashboardRepository _dashboardRepository;
        private readonly IQuickAccessRepository _quickAccessRepository;
        private readonly IUserQuickAccessRepository _userQuickAccessRepository;
        private readonly EncryptionService _encryptionService;

        public DashhboardServices(IQuickAccessRepository quickAccessRepository, IUserQuickAccessRepository userQuickAccessRepository, EncryptionService encryptionService, AccountService accountservice)
        {
            _quickAccessRepository = quickAccessRepository;
            _userQuickAccessRepository = userQuickAccessRepository;
            _encryptionService = encryptionService;

        }

        //Post Quick access
        #region Post Requests
        public async Task<ApiResponseNoData> AddQuickAccessMenu(QuickAccesMenuDto model)
        {

            var successful = await _quickAccessRepository.AddQuickAccessMenu(new QuickAccess()
            {
                NAME = model.NAME,
                ICON = model.ICON,
                ACTIVE = model.ACTIVE

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }


        //Save User By Quick Access
        public async Task<ApiResponseNoData> SaveUserQuickAccess(long userId, long quickAccessId)
        {
            // 1. Check if already exists
            var existing = await _userQuickAccessRepository.GetExisting(userId, quickAccessId);
            if (existing != null)
                throw new ApiException("Quick access already saved.");

            // 2. Check max limit
            var count = await _userQuickAccessRepository.GetCountByUserId(userId);
            if (count >= 4)
                throw new ApiException("You can only save up to 4 items");
            // 3. Save new entry
            var added = await _userQuickAccessRepository.Add(new UserQuickAccess
            {
                USERID = userId,
                QUICKACCESSID = quickAccessId
            });

            return added
                ? ApiResponseNoData.SuccessResponse("Saved successfully.")
                : ApiResponseNoData.FailureResponse("Failed to save.");
        }

        #endregion

        #region get Requests
        
        public async Task<ApiResponseWithData<List<QuickAccess>>> GetAllQuickAccessMenus()
        {
            var entities = await _quickAccessRepository.GetAllQuickAccessMenu();

            var dtoList = new List<QuickAccess>();

            foreach (var item in entities)
            {
                var dto = new QuickAccess
                {
                    ID = item.ID,
                    NAME = item.NAME,
                    ICON = item.ICON,
                    ACTIVE = item.ACTIVE
                };

                dtoList.Add(dto);
            }

            return ApiResponseWithData<List<QuickAccess>>.SuccessResponse(dtoList);
        }


        public async Task<ApiResponseWithData<UserQuickAccessResponseDto>> GetUserQuickAccessDetails(long userId)
        {
            var userQuickAccessIds = await _userQuickAccessRepository.GetQuickAccessIdsByUserId(userId);
            var allItems = await _quickAccessRepository.GetAllQuickAccessMenu();

            if (allItems == null || !allItems.Any())
            {
                throw new ApiException("No Quick Access menus found..");

            }

            var activeItems = allItems
                .Where(x => userQuickAccessIds.Contains(x.ID) && x.ACTIVE == 1)
                .Take(4)
                .Select(x => new QuickAccesMenuDto
                {
                    ID = x.ID,
                    NAME = x.NAME,
                    ICON = x.ICON
                    //ACTIVE = x.ACTIVE
                })
                .ToList();

            var availableItems = allItems
                .Where(x => !userQuickAccessIds.Contains(x.ID))
                .Select(x => new QuickAccesMenuDto
                {
                    ID = x.ID,
                    NAME = x.NAME,
                    ICON = x.ICON
                    //ACTIVE = x.ACTIVE
                })
                .ToList();

            var responseDto = new UserQuickAccessResponseDto
            {
                ActiveItems = activeItems,
                AvailableItems = availableItems
            };

            return ApiResponseWithData<UserQuickAccessResponseDto>.SuccessResponse(responseDto);
        }

        #endregion

        public async Task<ApiResponseNoData> RemoveUserQuickAccess(long userId, long quickAccessId)
        {
            var deleted = await _userQuickAccessRepository.Delete(userId, quickAccessId);

            if (!deleted)
                throw new ApiException("Record not found.");
               

            return ApiResponseNoData.SuccessResponse("Deleted successfully.");
        }


    }
}
