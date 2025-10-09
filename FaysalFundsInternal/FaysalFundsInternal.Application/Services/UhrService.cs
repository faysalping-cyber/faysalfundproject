using FaysalFundsInternal.Application.DTOs;
using FaysalFundsInternal.CrossCutting.Responses;
using FaysalFundsInternal.Domain.Interfaces;
using System.Collections.Generic;

namespace FaysalFundsInternal.Application.Services
{
    public class UhrService
    {
        private readonly IUHRRepository _uhrRepository;
        private readonly IUHSRepository _uhsRepository;
        private readonly UhsService _uhsService;
        public UhrService(IUHRRepository uhrRepository, IUHSRepository uhsRepository, UhsService uhsService)
        {
            _uhrRepository = uhrRepository;
            _uhsRepository = uhsRepository;
            _uhsService = uhsService;
        }

        public async Task<ApiResponse<List<CheckBalanceModel>>> CheckBalance(string cnic, string phoneNo, long? folio)
        {
            var checkBalanceList = new List<CheckBalanceModel>();
            if (folio != null && folio != 0)
            {
                checkBalanceList = await CheckBalanceAgainstFolio(cnic, phoneNo, folio.Value);
            }
            else
            {
                checkBalanceList = await CheckBalanceAgainstCnicAndCellNo(cnic, phoneNo);
            }
            return ApiResponse<List<CheckBalanceModel>>.Success(checkBalanceList);
        }

        private async Task<List<CheckBalanceModel>> CheckBalanceAgainstCnicAndCellNo(string cnic, string phoneNo)
        {
            var folioLists = await _uhsRepository.GetAvailableCustomerFolios(cnic, phoneNo);
            var uhr = await _uhrRepository.CheckBalanceAgainstListOfFolios(folioLists);
            if (uhr.Count==0)
            {
                throw new ApiException("No folio is found.");
            }
            var checkBalanceList = new List<CheckBalanceModel>();
            foreach (var item in uhr)
            {
                checkBalanceList.Add(new CheckBalanceModel
                {
                    FolioNo = item.FOLIONO,
                    BalanceAmount = item.BALANCE_AMOUNT,
                    FundName = item.FUND_NAME,
                    AccountType=item.ACCOUNTTYPE
                });
            }
            return checkBalanceList;
        }
        private async Task<List<CheckBalanceModel>> CheckBalanceAgainstFolio(string cnic, string phoneNo, long folio)
        {
            await _uhsService.DoesFolioExistAgainstGivenCNICAndPhoneNo(cnic, phoneNo, folio);
            await _uhsService.FilterFolio(folio);
            var uhr = await _uhrRepository.CheckBalanceAgainstFolioNo(folio);

            var checkBalanceList = new List<CheckBalanceModel>();
            foreach (var item in uhr)
            {
                checkBalanceList.Add(new CheckBalanceModel
                {
                    FolioNo = item.FOLIONO,
                    BalanceAmount = item.BALANCE_AMOUNT,
                    FundName = item.FUND_NAME,
                    AccountType = item.ACCOUNTTYPE
                });
            }
            return checkBalanceList;
        }
    }
}
