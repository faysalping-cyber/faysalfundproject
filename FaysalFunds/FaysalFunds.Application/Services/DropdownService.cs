using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Domain.Interfaces.Dropdowns;

namespace FaysalFunds.Application.Services
{
    public class DropdownService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IExpectedInvestmentRepository _expectedInvestmentRepository;
        private readonly IExpectedNOofRedumptionRepository _expectedNoofRedumptionRepository;
        private readonly IMobileOwnershipRepository _mobileOwnershipRepository;
        private readonly INoTINReasonRepository _oTINReasonRepository;
        private readonly IOccupationRepository _occupationRepository;
        private readonly ISourceOfIncomeRepository _sourceOfIncomeRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IBankRepository _bankRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IExpectedInvestmentRepository _expectednoInvestmentRepository;
        private readonly IExpectedMonthlyInvestmentAmountRepository _expectedMonthlyInvestmentAmountRepository;
        private readonly IExpectedMonthlyNoOfInvestmentTransactionRepository _expectedMonthlyNoOfInvestmentTransactionRepository;
        private readonly IExpectedMonthlyNoOfRedemptionTransactionRepository _expectedMonthlyNoOfRedemptionTransactionRepository;


        public DropdownService(
            ICountryRepository countryRepository,
            IExpectedInvestmentRepository expectedInvestmentRepository,
            IExpectedNOofRedumptionRepository expectedNoofRedumptionRepository,
            IMobileOwnershipRepository mobileOwnershipRepository,
            INoTINReasonRepository oTINReasonRepository,
            IOccupationRepository occupationRepository,
            IProfessionRepository professionRepository,
            ISourceOfIncomeRepository sourceOfIncomeRepository,
            ICityRepository cityRepository,
            IBankRepository bankRepository,
            IWalletRepository walletRepository,
            IExpectedInvestmentRepository expectednoInvestmentRepository,
            IExpectedMonthlyInvestmentAmountRepository expectedMonthlyInvestmentAmountRepository,
            IExpectedMonthlyNoOfInvestmentTransactionRepository expectedMonthlyNoOfInvestmentTransactionRepository,
            IExpectedMonthlyNoOfRedemptionTransactionRepository expectedMonthlyNoOfRedemptionTransactionRepository
            )
        {
            _countryRepository = countryRepository;
            _expectedInvestmentRepository = expectedInvestmentRepository;
            _expectedNoofRedumptionRepository = expectedNoofRedumptionRepository;
            _mobileOwnershipRepository = mobileOwnershipRepository;
            _oTINReasonRepository = oTINReasonRepository;
            _occupationRepository = occupationRepository;
            _sourceOfIncomeRepository = sourceOfIncomeRepository;
            _cityRepository = cityRepository;
            _bankRepository = bankRepository;
            _professionRepository = professionRepository;
            _walletRepository = walletRepository;
            _expectednoInvestmentRepository = expectednoInvestmentRepository;
            _expectedMonthlyInvestmentAmountRepository = expectedMonthlyInvestmentAmountRepository;
            _expectedMonthlyNoOfInvestmentTransactionRepository = expectedMonthlyNoOfInvestmentTransactionRepository;
            _expectedMonthlyNoOfRedemptionTransactionRepository = expectedMonthlyNoOfRedemptionTransactionRepository;
        }

        public async Task<ApiResponseWithData<Dictionary<string, List<DropDownDTO>>>> GetDropdownsAsync()
        {
            var dropdowns = new Dictionary<string, List<DropDownDTO>>
    {
        { "banks", (await _bankRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "cities", (await _cityRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "countries", (await _countryRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "expectedInvestments", (await _expectedInvestmentRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "expectedNoOfRedemptions", (await _expectedNoofRedumptionRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "mobileOwnerships", (await _mobileOwnershipRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "noTINReasons", (await _oTINReasonRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "occupations", (await _occupationRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "sourceOfIncomes", (await _sourceOfIncomeRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "profession", (await _professionRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        { "wallet", (await _walletRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        {"expectedNoInvestment",(await _expectednoInvestmentRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        {"expectedMonthlyInvestmentAmount",(await _expectedMonthlyInvestmentAmountRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        {"expectedMonthlyNoOfInvestmentTransaction",(await _expectedMonthlyNoOfInvestmentTransactionRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },
        {"expectedMonthlyNoOfRedemptionTransaction",(await _expectedMonthlyNoOfRedemptionTransactionRepository.GetAll()).Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE }).ToList() },


    };
            return ApiResponseWithData<Dictionary<string, List<DropDownDTO>>>.SuccessResponse(dropdowns);
        }

        public async Task<ApiResponseWithData<List<DropDownDTO>>> GetMobileOwnershipsAsync()
        {
            var mobileOwnerships = (await _mobileOwnershipRepository.GetAll())
                .Select(x => new DropDownDTO { Id = x.ID, Title = x.TEXT_VALUE })
                .ToList();

            return ApiResponseWithData<List<DropDownDTO>>.SuccessResponse(mobileOwnerships);
        }

    }
}