using FaysalFundsInternal.Application.DTOs;
using FaysalFundsInternal.CrossCutting.Responses;
using FaysalFundsInternal.Domain.Interfaces;
using System.Diagnostics.Eventing.Reader;

namespace FaysalFundsInternal.Application.Services
{
    public class AccountStatementService
    {
        private readonly IAccountStatementRepository _accountStatementRepository;
        private readonly IUnitHolderRepository _unitHolderRepository;
        private readonly UhsService _UhsService;

        public AccountStatementService(IAccountStatementRepository accountStatementRepository, IUnitHolderRepository unitHolderRepository, UhsService uhsService)
        {
            _accountStatementRepository = accountStatementRepository;
            _unitHolderRepository = unitHolderRepository;
            _UhsService = uhsService;
        }

        public async Task<ApiResponse<List<ProfitResponseModel>>> Profit(long folioNo, bool isFiscalYear)
        {
            List<ProfitResponseModel> responseModel = new List<ProfitResponseModel>();    
            if (isFiscalYear) {

                var response =await _accountStatementRepository.FiscalProfit(folioNo);
                foreach (var item in response)
                {
                    responseModel.Add(new ProfitResponseModel
                    {
                        Fund = item.Fund,
                        Profit = item.Profit
                    });
                }

            }
            else
            {
                var response = await _accountStatementRepository.ProfitSinceInception(folioNo);
                foreach (var item in response)
                {
                    responseModel.Add(new ProfitResponseModel
                    {
                        Fund = item.Fund,
                        Profit = item.Profit
                    });
                }
            }
            return ApiResponse<List<ProfitResponseModel>>.Success(responseModel);    
        }

        public async Task<ApiResponse<ProfitResponseModelForMobileApp>> Profit(string cnic, string phoneNo, long? folioNo)
        {
            ProfitResponseModelForMobileApp resposneModel = new ProfitResponseModelForMobileApp();
            if (folioNo is not null && folioNo != 0 )
            {
               await _UhsService.DoesFolioExistAgainstGivenCNICAndPhoneNo(cnic, phoneNo, folioNo.Value);
                var folioProfit = await GetFolioProfit(folioNo.Value);
                resposneModel.Profit = folioProfit;
                return ApiResponse<ProfitResponseModelForMobileApp>.Success(resposneModel);
            }

           List<long> folioList = await _UhsService.GetCustomerAvailableFolioForMobileApp(cnic, phoneNo);
            List<decimal> profitList = new List<decimal>();
            foreach (var folio in folioList)
            {
                var folioProfit = await GetFolioProfit(folio);
                profitList.Add(folioProfit);
            }
            resposneModel.Profit = profitList.Sum();

            return ApiResponse<ProfitResponseModelForMobileApp>.Success(resposneModel);
        }
        private async Task<decimal> GetFolioProfit(long folioNo)
        {
            var response = await _accountStatementRepository.FiscalProfit(folioNo);
           var sum = response.Sum(e => e.Profit);
            return sum;
        }
    }
}