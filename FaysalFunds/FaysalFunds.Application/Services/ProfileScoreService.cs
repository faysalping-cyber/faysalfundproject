using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;

namespace FaysalFunds.Application.Services
{
    public class ProfileScoreService
    {
        private readonly IProfileScoreRepository _profileScoreRepository;

        public ProfileScoreService(IProfileScoreRepository profileScoreRepository)
        {
            _profileScoreRepository = profileScoreRepository;
        }

        public async Task<ApiResponseNoData> Save(long accountOpeningId, int totalRiskScore)
        {
            var profileScoreModel = new ProfileScore()
            {
                ACCOUNTOPENING_ID = accountOpeningId,
                TOTAL_SCORE= totalRiskScore
            };
            var successful = await _profileScoreRepository.Upsert(profileScoreModel);
            if (!successful)
                throw new ApiException("Total Score Not Saved");
            return ApiResponseNoData.SuccessResponse();
        }
        public async Task<ApiResponseWithData<RiskScoreGetModel>> GetRiskProfile(long userId)
        {
            var result = await _profileScoreRepository.GetRiskProfile(userId);
            if (result == null)
                return ApiResponseWithData<RiskScoreGetModel>.FailureResponse("No score is found.");
            var scoreModel = new RiskScoreGetModel()
            {
                Score = result.TOTAL_SCORE,
                RiskLevel = result.RISKLEVEL
            };
            return ApiResponseWithData<RiskScoreGetModel>.SuccessResponse(scoreModel);
        }
    }
}
