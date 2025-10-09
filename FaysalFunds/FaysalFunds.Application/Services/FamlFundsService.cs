using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Domain.Interfaces;

namespace FaysalFunds.Application.Services
{
    public class FamlFundsService
    {
        private readonly IFamlFundRepository _famlFundRepository;

        public FamlFundsService(IFamlFundRepository famlFundRepository)
        {
            _famlFundRepository = famlFundRepository;
        }

        public async Task<ApiResponseWithData<ActiveFunds>> GetActiveFunds()
        {
            var response = await _famlFundRepository.GetActiveFunds();
            if (response == null)
                return ApiResponseWithData<ActiveFunds>.FailureResponse("No active fund available.");
            var activeList = new ActiveFunds();
            activeList.TransactionTypes = new List<Fund>();
            foreach (var item in response)
            {
                activeList.TransactionTypes.Add(new Fund
                {
                    Id = item.ID,
                    Title = item.TITLE,
                    AllTimeInvestmentLimit = item.ALLTIMEINVESTMENTLIMIT,
                    AnnualInvestmentLimit = item.ALLTIMEINVESTMENTLIMIT,
                    PerTransactionLimit = item.PERTRANSACTIONLIMIT
                });
            }
            return ApiResponseWithData<ActiveFunds>.SuccessResponse(activeList);
        }
        public async Task IsFundExists(int fundId)
        {
            var isFundExists = await _famlFundRepository.IsFundExistsAgainstId(fundId);
            if (!isFundExists)
            {
                throw new ApiException("Fund Id does not exist.");
            }
        }
    }
}
