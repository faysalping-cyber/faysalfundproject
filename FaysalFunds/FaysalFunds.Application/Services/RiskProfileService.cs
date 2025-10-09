using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Domain.Interfaces.Dropdowns;
namespace FaysalFunds.Application.Services
{
    public class RiskProfileService
    {
        private readonly IAgeRepository _ageRepository;
        private readonly IMartialStatusRepository _martialStatusRepository;
        private readonly INumberOfDependentsRepository _numberOfDependentsRepository;
        private readonly IOccupationRepository _occupationRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IRiskAppititeRepository _riskAppititeRepository;
        private readonly IInvestmentObjectivesRepository _investmentObjectivesRepository;
        private readonly IInvestmentHorizonRepository _investmentHorizonRepository;
        private readonly IInvestmentKnowledgeRepository _investmentKnowledgeRepository;
        private readonly IFinancialPositionRepository _financialPositionRepository;
        private readonly IRiskProfileOccupationRepository _riskProfileOccupationRepository;

        public RiskProfileService(
            IAgeRepository ageRepository,
            IMartialStatusRepository martialStatusRepository,
            INumberOfDependentsRepository numberOfDependentsRepository,
            IOccupationRepository occupationRepository,
            IEducationRepository educationRepository,
            IRiskAppititeRepository riskAppititeRepository,
            IInvestmentObjectivesRepository investmentObjectivesRepository,
            IInvestmentHorizonRepository investmentHorizonRepository,
            IInvestmentKnowledgeRepository investmentKnowledgeRepository,
            IFinancialPositionRepository financialPositionRepository,
            IRiskProfileOccupationRepository riskProfileOccupationRepository)
        {
            _ageRepository = ageRepository;
            _martialStatusRepository = martialStatusRepository;
            _numberOfDependentsRepository = numberOfDependentsRepository;
            _occupationRepository = occupationRepository;
            _educationRepository = educationRepository;
            _riskAppititeRepository = riskAppititeRepository;
            _investmentObjectivesRepository = investmentObjectivesRepository;
            _investmentHorizonRepository = investmentHorizonRepository;
            _investmentKnowledgeRepository = investmentKnowledgeRepository;
            _financialPositionRepository = financialPositionRepository;
            _riskProfileOccupationRepository = riskProfileOccupationRepository;
        }

        public async Task<ApiResponseWithData<RiskProfileDropDownDTO>> GetRiskProfileDropdownsAsync()
        {
            var dropdownConfigs = new List<(string Key, string Question, Func<Task<IEnumerable<DropDownDTO>>> GetItems)>
        {
            ("age", "Age", async () =>
                (await _ageRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("maritalStatus", "Martial Status", async () =>
                (await _martialStatusRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("noOfDependents", "Number Of Dependents", async () =>
                (await _numberOfDependentsRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("occupation", "Occupation", async () =>
                (await _riskProfileOccupationRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("education", "Education", async () =>
                (await _educationRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("riskAppetite", "Risk Appetite", async () =>
                (await _riskAppititeRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("investmentObjective", "Investment Objectives", async () =>
                (await _investmentObjectivesRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("investmentHorizon", "Investment Horizon", async () =>
                (await _investmentHorizonRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("investmentKnowledge", "Investment Knowledge", async () =>
                (await _investmentKnowledgeRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })),
            ("financialPosition", "Financial Position", async () =>
                (await _financialPositionRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }))
        };

            var dropdowns = new List<QuestionnaireItemDTO>();

            foreach (var config in dropdownConfigs)
            {
                var answers = (await config.GetItems()).ToList();
                dropdowns.Add(new QuestionnaireItemDTO
                {
                    Key = config.Key,
                    Question = config.Question,
                    Answers = answers
                });
            }

            return ApiResponseWithData<RiskProfileDropDownDTO>.SuccessResponse(
                new RiskProfileDropDownDTO { Body = dropdowns }
            );
        }
    }
}