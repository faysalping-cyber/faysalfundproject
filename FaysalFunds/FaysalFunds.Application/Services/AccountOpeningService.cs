using DocumentFormat.OpenXml.EMMA;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.DTOs.AccountOpening.AccountSelection;
using FaysalFunds.Application.DTOs.AccountOpening.BasicInformation;
using FaysalFunds.Application.DTOs.AccountOpening.RegulatoryKYC;
using FaysalFunds.Application.DTOs.AccountOpening.UploadDocument;
using FaysalFunds.Application.Services.IServices;
using FaysalFunds.Application.Utilities;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Common.Enums;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
namespace FaysalFunds.Application.Services
{
    public class AccountOpeningService
    {
        private readonly Settings _settings;
        private readonly DateParserUtility _dateParser;
        private readonly IAccountOpeningRepository _accountOpeningRepository;
        private readonly AccountService _accountservice;
        private readonly FamlFundsService _famlFundsService;
        private readonly ProfileScoreService _profileScoreService;
        private readonly IFamlFundRepository _famlFundRepository;
        private readonly DropdownService _dropdownService;
        private readonly IEmailService _emailService;
        public AccountOpeningService(IAccountOpeningRepository accountOpeningRepository, IFamlFundRepository famlFundRepository, AccountService accountservice, FamlFundsService famlFundsService, DateParserUtility dateParser, ProfileScoreService profileScoreService, DropdownService dropdownService, IEmailService emailService, IOptions<Settings> settings)
        {
            _accountOpeningRepository = accountOpeningRepository;
            _famlFundRepository = famlFundRepository;
            _accountservice = accountservice;
            _famlFundsService = famlFundsService;
            _dateParser = dateParser;
            _profileScoreService = profileScoreService;
            _dropdownService = dropdownService;
            _settings = settings.Value;
            _emailService = emailService;

        }

        public async Task<ApiResponseNoData> AddAccountSelection(AccountSelectionRequestModel model)
        {
            //await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            //await _accountservice.IsAccountExistAgainstUserId(model.UserId);

            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            //await DoesAnEntryExistsAgainstUserId(model.UserId);
            await _famlFundsService.IsFundExists(model.AccountType);
            long id = await GetOnBoardingIdByUserId(model.UserId);
            
            var successful = await _accountOpeningRepository.AddAccountSelection(new AccountOpening()
            {
                ID=id,
                ACCOUNTID = model.UserId,
                ACCOUNT_TYPE = model.AccountType
            });
            if (!successful)
            {
                return ApiResponseNoData.FailureResponse();
            }
            else
            {
                var savedData = await GetSavedAccountSelection(model.UserId);
                return ApiResponseNoData.SuccessResponse();
            }
        }
        public async Task<ApiResponseNoData> SaveBasicInformationStep1(BasicInformation1 model)
        {
            //await CheckOnBoardingStatus(model.UserId);
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            await IsBasicInfoEnabled(model.UserId);
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);

            var successful = await _accountOpeningRepository.SaveBasicInformationStep1(new AccountOpening()
            {

                ID = id,
                TITLE_FULL_NAME = model.FullName,
                FATHERORHUSBANDNAME = model.FatherOrHusbandName,
                MOTHER_NAME = model.MotherName,
                PRINCIPLE_CNIC = model.CNIC,
                CNIC_ISSUANCE_DATE = _dateParser.ParseDate(model.CNIC_IssueDate),
                PRINCIPLE_CNIC_EXP_DATE = model.IsCnicExpiryLifetime == 1 ? null : _dateParser.ParseDate(model.CNIC_ExpiryDate),
                PRINCIPLE_CNIC_EXP_LIFE_TIME = model.IsCnicExpiryLifetime,
                BASICINFO_STEP1 = 1 // indication for step completion

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }

        public async Task<ApiResponseNoData> SaveBasicInformationStep2(BasicInformation2 model)
        {
            //await CheckOnBoardingStatus(model.UserId);
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            await IsBasicInfoEnabled(model.UserId);
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);
            var successful = await _accountOpeningRepository.SaveBasicInformationStep2(new AccountOpening()
            {
                ID = id,
                DATE_OF_BIRTH = _dateParser.ParseDate(model.DOB),
                PLACE_OF_BIRTH = model.PlaceOfBirth,
                GENDER = model.Gender,
                RESIDENTIAL_STATUS = model.ResidentialStatus,
                ZAKAT_STATUS = model.ZakatStatus,
                UPLOADCZ_50 = model.UploadCZ_50,
                REFERRALCODE = model.ReferralCode,
                BASICINFO_STEP2 = 1 // indication for step completion

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }

        public async Task<ApiResponseNoData> SaveContactDetail(ContactDetail model)
        {
            //await CheckOnBoardingStatus(model.UserId);
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            await IsPersonalDetailsEnabled(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);
            var successful = await _accountOpeningRepository.SaveContactDetail(new AccountOpening()
            {
                ID = id,
                PERMANENT_ADDRESS = model.PermanentAddress,
                PERMANENT_CITY = model.City,
                PERMANENT_COUNTRY = model.Country,
                IS_MAILING_SAME_AS_PERMANENT_ADDRESS = model.MailingAddressSameAsPermanent,
                MAILING_CITY = model.MalingCity == 0 ? null : model.MalingCity,
                MAILING_COUNTRY = model.MalingCountry == 0 ? null : model.MalingCountry,

                MAILING_ADDRESS_1 = model.MalingAddress1,
                MOB_NO_COUNTRY_CODE = model.CountryCode,
                MOBILE_NUMBER = model.MobileNumber,
                //MOBILE_NUMBER_OWNERSHIP = model.MobileNumberOwnership,
                MOBILE_NUMBER_OWNERSHIP = model.MobileNumberOwnership ==0 ? null : model.MobileNumberOwnership,
                EMAIL = model.Email,
                PERSONALDETAILS_STEP1 = 1 // indication for step completion

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }
        public async Task<ApiResponseNoData> SaveNextToKinInformation(NextToKinInfo model)
        {
            //await CheckOnBoardingStatus(model.UserId);
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            await IsPersonalDetailsEnabled(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);
            var successful = await _accountOpeningRepository.SaveNextToKinInformation(new AccountOpening()
            {
                ID = id,
                NEXTOFKIN_NAME = model.NextOfKinName,
                NEXTOFKIN_NO_COUNTRY_CODE = model.CountryCode,
                NEXTOFKIN_MOB_NUMBER = model.ContactNo,
                PERSONALDETAILS_STEP2 = 1 // indication for step completion
            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }
        public async Task<ApiResponseNoData> SaveBankAccountInformation(BankDetails model)
        {
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            await IsPersonalDetailsEnabled(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);
            if (model.TypeOfAccount?.ToLower() == "wallet" && model.Bank == 0)
            {
                model.Bank = null;
            }
            if (model.TypeOfAccount?.ToLower() == "bank" && model.Wallet ==0)
            {
                model.Wallet = null;
            }
            var successful = await _accountOpeningRepository.SaveBankDetails(new AccountOpening()
            {
                ID = id,
                TYPE_OF_ACCOUNT = model.TypeOfAccount,
                BANK_NAME = model.Bank,
                IBAN_NO = model.IBAN,
                WALLET_NAME = model.Wallet,
                WALLET_NO = model.WalletNumber,
                DIVIDEND_PAYOUT = model.DividendPayout,
                PERSONALDETAILS_STEP3 = 1 // indication for step completion

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }



        public async Task<ApiResponseNoData> SaveRiskProfileDropdownValues(RiskProfile model)
        {
            //await CheckOnBoardingStatus(model.UserId);
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            await IsRiskProfileEnabled(model.UserId);
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);
            var successful = await _accountOpeningRepository.SaveRiskProfileDropdownValues(new AccountOpening()
            {
                ID = id,
                AGE = model.Age,
                MARITAL_STATUS = model.MaritalStatus,
                NO_OF_DEPENDENTS = model.NoOfDependents,
                RISKPROFILE_OCCUPATION = model.Occupation,
                EDUCATION = model.Education,
                RISK_APPETITE = model.RiskAppetite,
                INVESTMENT_OBJECTIVE = model.InvestmentObjective,
                INVESTMENT_HORIZON = model.InvestmentHorizon,
                INVESTMENT_KNOWLEDGE = model.InvestmentKnowledge,
                FINANCIALPOSITION = model.FinancialPosition,
                RISK_PROFILE_DECLARATION = model.RiskProfileDeclaration,
                RISKPROFILE_STEP = 1 // indication for step completion

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            //Risk Calculation working
            var riskTotalScore = await _accountOpeningRepository.CalculateRiskProfileScore(id);
            await _profileScoreService.Save(id, riskTotalScore);
            return ApiResponseNoData.SuccessResponse("Saved successfully");
        }

        public async Task<ApiResponseNoData> SaveRegulatoryKYC_Step1(RegulatoryKYC1 model)
        {
            //await CheckOnBoardingStatus(model.UserId);

            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            await IsRegulatoryKYCEnabled(model.UserId);
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);

            var nullOccupationIds = new List<int> { 3, 4, 5, 6 }; // Student, House Wife, Retired, Unemployed

            if (!nullOccupationIds.Contains(model.Occupation) &&
                (model.Profession == 0))
            {
                throw new ApiException("Profession or Name of Employer is required.");
            }

            int? profession = nullOccupationIds.Contains(model.Occupation) ? null : model.Profession;
            //int? nameOfEmployerOrBusiness = nullOccupationIds.Contains(model.Occupation) ? null : model.NameOfEmployerOrBussiness;

            var successful = await _accountOpeningRepository.SaveRegulatoryKYC1(new AccountOpening()
            {
                ID = id,
                OCCUPATION = model.Occupation,
                PROFESSION = profession,
                SOURCE_OF_INCOME = model.SourceOfIncome,
                NAME_OF_EMPLOYER_OR_BUSINESS = model.NameOfEmployerOrBussiness,
                GROSS_ANNUAL_INCOME = model.GrossAnnualIncome,
                EXPECTED_MONTHLY_INVESTEMENTAMOUNT = model.MonthlyExpectedInvestmentAmount,
                EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION = model.MonthlyExpectedNoOfInvestmentTransaction,
                EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION = model.MonthlyExpectedNoOfRedemptionTransaction,
                REGULATORYKYC_STEP1 = 1

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }
        public async Task<ApiResponseNoData> SaveRegulatoryKYC_Step2(RegulatoryKYC2 model)
        {
            //await CheckOnBoardingStatus(model.UserId);

            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            await IsRegulatoryKYCEnabled(model.UserId);
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);
            var successful = await _accountOpeningRepository.SaveRegulatoryKYC2(new AccountOpening()
            {
                ID = id,
                FINANCIAL_INSTITUUTION_REFUSAL = model.FinantialInstitutionRefusal,
                ULTIMATE_BENEFICIARY = model.UltimateBeneficiary,
                HOLDING_SENIOR_POSITION = model.HoldingSeniorPosition,
                INTERNATIONAL_RELATION_OR_PEP = model.InternationalRelationshipOrPREP,
                DEAL_WITH_HIGH_VALUE_ITEMS = model.DealingWithHighValueItems,
                FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS = model.FinancialLinksWithOffshoreTaxHavens,
                TRUE_INFORMATION_DECLARATION = model.TrueInformationDeclaration,
                REGULATORYKYC_STEP2 = 1

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }
        public async Task<ApiResponseNoData> SaveFATCA(FATCA model)
        {
            //await CheckOnBoardingStatus(model.UserId);
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            await IsRegulatoryKYCEnabled(model.UserId);
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);

            var successful = await _accountOpeningRepository.SaveFATCA(new AccountOpening()
            {
                ID = id,
                COUNTRY_OF_RESIDENT = model.CountryOfTaxResidenceElsePakistan,
                IS_US_CITIZEN_RESIDENT_HAVEGREENCARD = model.IsUS_CitizenResidentOrHaveGreenCard,
                US_BORN = model.IsUs_Born,
                INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA = model.InstructionsToTransferFundsToUSA,
                HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS = model.HaveUS_ResidenceMailingOrHoldingAddress,
                HAVE_US_TELEPHONE_NUMBER = model.HaveUS_TelephoneNumber,
                US_TAXPAYER_IDENTIFICATION_NUMBER = model.US_TaxPayerIdentificationNumber,
                W9_FORM_UPLOAD = model.W9FormUpload,
                FATCA_DECLARATION = model.FATCA_Declaration,
                NON_US_PERSON_DECLARATION = model.NonUS_PersonDeclaration,
                REGULATORYKYC_STEP3 = 1

            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }
        public async Task<ApiResponseNoData> SaveCRS(CRS model)
        {
            //await CheckOnBoardingStatus(model.UserId);
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
            {
                throw new ApiException(statusMessage);
            }
            await IsRegulatoryKYCEnabled(model.UserId);
            // await _accountservice.CheckLFDUserScreeningEntryInDB(model.UserId);
            long id = await GetAccountOpeningIdByUserId(model.UserId);
            if (model.TaxResidentCountry == 1)
            {
                model.TIN_Number = null;
                model.ReasonForNoTIN = null;
            }
            var successful = await _accountOpeningRepository.SaveCRS(new AccountOpening()
            {
                ID = id,
                TAX_RESIDENT_COUNTRY = model.TaxResidentCountry,
                TIN_NO = model.TIN_Number,
                REASON_FOR_NO_TIN_NUMBER = model.ReasonForNoTIN == 0 ? null : model.ReasonForNoTIN,
                HAVE_TIN = model.HaveTIN,
                CRS_DECLARATION = model.CRS_Declaration,
                REGULATORYKYC_STEP4 = 1
            });
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }
        public async Task<ApiResponseNoData> PostOnboardingProfile(OnboardingSubmitStatusDTO model)
        {
            long id = await GetAccountOpeningIdByUserId(model.UserId);

            var successful = await _accountOpeningRepository.PostOnboardingProfile(new AccountOpening()
            {
               ID=id,
               STATUS=model.STATUS

            });
            //var onboardingData = await _accountOpeningRepository.GetAccountOpening(model.UserId);
            //await _emailService.SendEmailAsync("hafizjunaid971@gmail.com", onboardingData);
            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

          
            return (ApiResponseNoData.SuccessResponse("Saved successfully"));
        }
        public async Task<ApiResponseWithData<MissingStepsResponseModel>> GetMissingOnboardingSteps(long userId)
        {
            var stepsData = await _accountOpeningRepository.StepsCounts(userId);
            var accountTypeID = await _accountOpeningRepository.GetAccountSelection(userId);
            var accountType = await _famlFundRepository.GetByActiveFundId((long)accountTypeID.ACCOUNT_TYPE);

            // get status
            var accountStatusResponse = await OnBoardingStatusAccounttype(userId);

            MissingStepsResponseModel response = new MissingStepsResponseModel
            {
                ProfileName = accountType.TITLE,
                MissingSteps = new List<string>(),
                OnboardingStatusMessage = accountStatusResponse.Data?.Message,
                Status = accountStatusResponse.Data?.Status
            };

            foreach (var step in stepsData)
            {
                if (step.CompletedSteps < step.TotalSteps)
                {
                    response.MissingSteps.Add(step.Title);
                }
            }

            response.Message = response.MissingSteps.Any()
                ? "Some onboarding steps are still missing."
                : "All onboarding steps completed.";

            return ApiResponseWithData<MissingStepsResponseModel>.SuccessResponse(response);
        }


        public async Task<ApiResponseNoData> UploadProfileDocuments(UploadDocuments model)
        {
            var statusMessage = await CheckOnBoardingStatusAccounttype(model.UserId);
            if (statusMessage != null)
                throw new ApiException(statusMessage);

            await IsUploadDocumentEnabled(model.UserId);

            var accountType = await _accountOpeningRepository.GetAccountSelection(model.UserId);
            if (accountType == null)
                throw new ApiException("No Account Type Found");

            var isDigitalFund = await _famlFundRepository.GetByActiveFundId((long)accountType.ACCOUNT_TYPE);

            var ownership = await _accountOpeningRepository.GetContactDetail(model.UserId);
            var ownershipDropdown = await _dropdownService.GetMobileOwnershipsAsync();
            if (ownershipDropdown.Data == null)
                throw new ApiException("Mobile ownership not found.");

            var ownershipTitle = ownershipDropdown.Data
                .FirstOrDefault(x => x.Id == ownership.MOBILE_NUMBER_OWNERSHIP)?.Title ?? "";

            // Agar Digital Fund nahi hai to extra documents required
            if (isDigitalFund.TITLE == "Sahulat Sarmayakari Account")
            {
                if (model.ProofOfSourceIncome != null && model.ProofOfSourceIncome.Length > 0)
                    throw new ApiException("Proof Source of Income is not required for Sahulat Sarmayakari Account.");

                if (model.PastOneYearBankstatement != null && model.PastOneYearBankstatement.Length > 0)
                    throw new ApiException("Past one year Bank Statement is not required for Sahulat Sarmayakari Account.");
            }
            else
            {
                if (model.ProofOfSourceIncome == null || model.ProofOfSourceIncome.Length == 0)
                    throw new ApiException("Proof Source of Income is required.");

                if (model.PastOneYearBankstatement == null || model.PastOneYearBankstatement.Length == 0)
                    throw new ApiException("Past one year Bank Statement is required.");
            }


            long id = await GetAccountOpeningIdByUserId(model.UserId);
          
            var successful = await _accountOpeningRepository.UploadProfileDocuments(new AccountOpening()
            {
                ID = id,
                CNIC_UPLOAD_FRONT = model.CnicUploadFront,
                CNIC_UPLOAD_BACK = model.CnicUploadBack,
                LIVEPHOTE_OR_SELFIE = model.LivePhotoOrSelfie,
                PROOF_OF_SOURCE_OF_INCOME = model.ProofOfSourceIncome,
                PAST_ONE_YEAR_BANKSTATEMENT = model.PastOneYearBankstatement,
                UPLOADDOCUMENT_STEP = 1,
                FAMILY_MEMBER = model.FamilayMember,
                INTERNATIONAL_NUMBER = model.InternationalNumber,
                COMPANY_REGISTER = model.CompanyRegister
            });

            if (!successful)
                return ApiResponseNoData.FailureResponse("Failed");

            return ApiResponseNoData.SuccessResponse("Saved successfully");
        }

        
        // get

        public async Task<ApiResponseWithData<AccountSelectionResponseModel>> GetAccountSelection(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetAccountSelection(model.UserId);
            if (response == null)
                return ApiResponseWithData<AccountSelectionResponseModel>.FailureResponse("Failed");
            var responseModel = new AccountSelectionResponseModel
            {
                AccountType = response.ACCOUNT_TYPE,
            };
            return ApiResponseWithData<AccountSelectionResponseModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<BasicInformation1GetModel>> GetBasicInformationStep1(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetBasicInformationStep1(model.UserId);
            if (response == null)
                return ApiResponseWithData<BasicInformation1GetModel>.FailureResponse("Failed");
            var responseModel = new BasicInformation1GetModel
            {
                FullName = response.TITLE_FULL_NAME,
                FatherOrHusbandName = response.FATHERORHUSBANDNAME,
                MotherName = response.MOTHER_NAME,
                CNIC = response.PRINCIPLE_CNIC,
                //CNIC_IssueDate = response.CNIC_ISSUANCE_DATE?.ToString(_settings.DateFormat),
                //CNIC_ExpiryDate = response.PRINCIPLE_CNIC_EXP_DATE?.ToString(_settings.DateFormat),
                CNIC_IssueDate = response.CNIC_ISSUANCE_DATE?.ToString("dd-MM-yyyy"),
                CNIC_ExpiryDate = response.PRINCIPLE_CNIC_EXP_DATE?.ToString("dd-MM-yyyy"),
                IsCnicExpiryLifetime = response.PRINCIPLE_CNIC_EXP_LIFE_TIME
            };
            return ApiResponseWithData<BasicInformation1GetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<BasicInformation2GetModel>> GetBasicInformationStep2(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetBasicInformationStep2(model.UserId);
            if (response == null)
                return ApiResponseWithData<BasicInformation2GetModel>.FailureResponse("Failed");
            var responseModel = new BasicInformation2GetModel
            {
                DOB = response.DATE_OF_BIRTH?.ToString("dd-MM-yyyy"),
                Gender = response.GENDER,
                PlaceOfBirth = response.PLACE_OF_BIRTH,
                ReferralCode = response.REFERRALCODE,
                ResidentialStatus = response.RESIDENTIAL_STATUS,
                UploadCZ_50 = response.UPLOADCZ_50,
                ZakatStatus = response.ZAKAT_STATUS

            };
            return ApiResponseWithData<BasicInformation2GetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<ContactDetailGetModel>> GetContactDetail(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetContactDetail(model.UserId);
            if (response == null)
                return ApiResponseWithData<ContactDetailGetModel>.FailureResponse("Failed");
            var responseModel = new ContactDetailGetModel
            {
                Country = response.PERMANENT_COUNTRY,
                City = response.PERMANENT_CITY,
                CountryCode = response.MOB_NO_COUNTRY_CODE,
                Email = response.EMAIL,
                MailingAddressSameAsPermanent = response.IS_MAILING_SAME_AS_PERMANENT_ADDRESS,
                PermanentAddress = response.PERMANENT_ADDRESS,
                MobileNumber = response.MOBILE_NUMBER,
                MobileNumberOwnership = response.MOBILE_NUMBER_OWNERSHIP,
                MalingCity = response.MAILING_CITY,
                MalingCountry = response.MAILING_COUNTRY,
                MalingAddress1 = response.MAILING_ADDRESS_1

            };
            return ApiResponseWithData<ContactDetailGetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<BankDetailsGetModel>> GetBankDetails(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetBankDetails(model.UserId);
            if (response == null)
                return ApiResponseWithData<BankDetailsGetModel>.FailureResponse("Failed");
            var responseModel = new BankDetailsGetModel
            {
                Bank = response.BANK_NAME,
                TypeOfAccount = response.TYPE_OF_ACCOUNT,
                Wallet = response.WALLET_NAME,
                WalletNumber = response.WALLET_NO,
                IBAN = response.IBAN_NO,
                DividendPayout = response.DIVIDEND_PAYOUT
            };
            return ApiResponseWithData<BankDetailsGetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<NextToKinInfoGetModel>> GetNextToKinInfo(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetNextToKinInformation(model.UserId);
            if (response == null)
                return ApiResponseWithData<NextToKinInfoGetModel>.FailureResponse("Failed");
            var responseModel = new NextToKinInfoGetModel
            {
                NextOfKinName = response.NEXTOFKIN_NAME,
                CountryCode = response.NEXTOFKIN_NO_COUNTRY_CODE,
                ContactNo = response.NEXTOFKIN_MOB_NUMBER
            };
            return ApiResponseWithData<NextToKinInfoGetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<RiskProfile>> GetRiskProfileSavedValues(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetRiskProfileSavedValues(model.UserId);
            if (response == null)
                return ApiResponseWithData<RiskProfile>.FailureResponse("Failed");
            var responseModel = new RiskProfile
            {
                Age = response.AGE,
                Education = response.EDUCATION,
                FinancialPosition = response.FINANCIALPOSITION,
                InvestmentHorizon = response.INVESTMENT_HORIZON,
                InvestmentKnowledge = response.INVESTMENT_KNOWLEDGE,
                InvestmentObjective = response.INVESTMENT_OBJECTIVE,
                MaritalStatus = response.MARITAL_STATUS,
                NoOfDependents = response.NO_OF_DEPENDENTS,
                Occupation = response.RISKPROFILE_OCCUPATION,
                RiskAppetite = response.RISK_APPETITE,
                RiskProfileDeclaration = response.RISK_PROFILE_DECLARATION,
            };
            return ApiResponseWithData<RiskProfile>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<RegulatoryKYC_Step1GetModel>> GetRegulatoryKYC_Step1(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetRegulatoryKYC1(model.UserId);
            if (response == null)
                return ApiResponseWithData<RegulatoryKYC_Step1GetModel>.FailureResponse("Failed");
            var responseModel = new RegulatoryKYC_Step1GetModel
            {
                Occupation = response.OCCUPATION,
                Profession = response.PROFESSION,
                SourceOfIncome = response.SOURCE_OF_INCOME,
                NameOfEmployerOrBussiness = response.NAME_OF_EMPLOYER_OR_BUSINESS,
                GrossAnnualIncome = response.GROSS_ANNUAL_INCOME,
                MonthlyExpectedInvestmentAmount = response.EXPECTED_MONTHLY_INVESTEMENTAMOUNT,
                MonthlyExpectedNoOfInvestmentTransaction = response.EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION,
                MonthlyExpectedNoOfRedemptionTransaction = response.EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION
            };
            return ApiResponseWithData<RegulatoryKYC_Step1GetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<RegulatoryKYC_Step2GetModel>> GetRegulatoryKYC_Step2(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetRegulatoryKYC2(model.UserId);
            if (response == null)
                return ApiResponseWithData<RegulatoryKYC_Step2GetModel>.FailureResponse("Failed");
            var responseModel = new RegulatoryKYC_Step2GetModel
            {
                FinantialInstitutionRefusal = response.FINANCIAL_INSTITUUTION_REFUSAL,
                UltimateBeneficiary = response.ULTIMATE_BENEFICIARY,
                HoldingSeniorPosition = response.HOLDING_SENIOR_POSITION,
                InternationalRelationshipOrPREP = response.INTERNATIONAL_RELATION_OR_PEP,
                DealingWithHighValueItems = response.DEAL_WITH_HIGH_VALUE_ITEMS,
                FinancialLinksWithOffshoreTaxHavens = response.FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS,
                TrueInformationDeclaration = response.TRUE_INFORMATION_DECLARATION,
            };
            return ApiResponseWithData<RegulatoryKYC_Step2GetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<FATCA_GetModel>> GetFATCA(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetFATCA(model.UserId);
            if (response == null)
                return ApiResponseWithData<FATCA_GetModel>.FailureResponse("Failed");
            var responseModel = new FATCA_GetModel
            {
                CountryOfTaxResidenceElsePakistan = response.COUNTRY_OF_RESIDENT,
                IsUS_CitizenResidentOrHaveGreenCard = response.IS_US_CITIZEN_RESIDENT_HAVEGREENCARD,
                IsUs_Born = response.US_BORN,
                InstructionsToTransferFundsToUSA = response.INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA,
                HaveUS_ResidenceMailingOrHoldingAddress = response.HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS,
                HaveUS_TelephoneNumber = response.HAVE_US_TELEPHONE_NUMBER,
                US_TaxPayerIdentificationNumber = response.US_TAXPAYER_IDENTIFICATION_NUMBER,
                W9FormUpload = response.W9_FORM_UPLOAD,
                FATCA_Declaration = response.FATCA_DECLARATION,
                NonUS_PersonDeclaration = response.NON_US_PERSON_DECLARATION
            };
            return ApiResponseWithData<FATCA_GetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<CRS_GetModel>> GetCRS(AccountOpeningRequestModel model)
        {
            var response = await _accountOpeningRepository.GetCRS(model.UserId);
            if (response == null)
                return ApiResponseWithData<CRS_GetModel>.FailureResponse("Failed");
            var responseModel = new CRS_GetModel
            {
                TaxResidentCountry = response.TAX_RESIDENT_COUNTRY,
                TIN_Number = response.TIN_NO,
                ReasonForNoTIN = response.REASON_FOR_NO_TIN_NUMBER,
                HaveTIN = response.HAVE_TIN,
                CRS_Declaration = response.CRS_DECLARATION,
            };
            return ApiResponseWithData<CRS_GetModel>.SuccessResponse(responseModel);
        }

        public async Task<ApiResponseWithData<UploadDocumentsGetModel>> GetProfileUploadDocuments(long userId)
        {
            var response = await _accountOpeningRepository.GetUploadedDocuments(userId);
            if (response == null)
                return ApiResponseWithData<UploadDocumentsGetModel>.FailureResponse("Failed to retrieve uploaded documents.");

            var ownership = await _accountOpeningRepository.GetContactDetail(userId);
            var ownershipDropdown = await _dropdownService.GetMobileOwnershipsAsync();

            if (ownershipDropdown.Data == null)
                throw new Exception("Mobile ownership not found.");

            var ownershipTitle = ownershipDropdown.Data
                .FirstOrDefault(x => x.Id == ownership.MOBILE_NUMBER_OWNERSHIP)?.Title ?? "";

            var accountType = await _accountOpeningRepository.GetAccountSelection(userId);
            if (accountType == null)
                throw new ApiException("No Account Type Found");

            var isDigitalFund = await _famlFundRepository.GetByActiveFundId((long)accountType.ACCOUNT_TYPE);

            var body = new List<UploadDocumentItemDTO>
    {
        new UploadDocumentItemDTO { Key = "cnicUploadFront", Title = "CNIC Front", File = response.CNIC_UPLOAD_FRONT, Subtext = "" },
        new UploadDocumentItemDTO { Key = "cnicUploadBack", Title = "CNIC Back", File = response.CNIC_UPLOAD_BACK, Subtext = "" },
        new UploadDocumentItemDTO { Key = "livePhotoOrSelfie", Title = "Live Photo or Selfie", File = response.LIVEPHOTE_OR_SELFIE, Subtext = "" }
    };

            if (isDigitalFund.TITLE != "Sahulat Sarmayakari Account")
            {
                body.Add(new UploadDocumentItemDTO
                {
                    Key = "proofOfSourceIncome",
                    Title = "Proof of Source of Income",
                    File = response.PROOF_OF_SOURCE_OF_INCOME,
                    Subtext = "Eg. Salary Slip, Business Letterhead etc."
                });

                body.Add(new UploadDocumentItemDTO
                {
                    Key = "pastOneYearBankstatement",
                    Title = "Bank Statement",
                    File = response.PAST_ONE_YEAR_BANKSTATEMENT,
                    Subtext = "For the period of past 1 year"
                });
                if (ownershipTitle == "Family Member")
                {
                    body.Add(new UploadDocumentItemDTO
                    {
                        Key = "familyMemberProof",
                        Title = "Family Member Document",
                        File = response.FAMILY_MEMBER,
                        Subtext = "Please provide a declaration stating the number you are using is of a family member"
                    });
                }
                else if (ownershipTitle == "Company Registered")
                {
                    body.Add(new UploadDocumentItemDTO
                    {
                        Key = "companyRegisterProof",
                        Title = "Company Registered Document",
                        File = response.COMPANY_REGISTER,
                        Subtext = "Please provide a bill of service provider / cover letter from employer"
                    });
                }
                else if (ownershipTitle == "International Number")
                {
                    body.Add(new UploadDocumentItemDTO
                    {
                        Key = "internationalNumberProof",
                        Title = "International Number Document",
                        File = response.INTERNATIONAL_NUMBER,
                        Subtext = "Please provide a bill of your service provider"
                    });
                }
            }

            var result = new UploadDocumentsGetModel
            {
                Body = body
            };

            return ApiResponseWithData<UploadDocumentsGetModel>.SuccessResponse(result);
        }

        public async Task<ApiResponseWithData<CompletedStepsCountModel>> StepsCounts(long userId)
        {
            CompletedStepsCountModel m = new CompletedStepsCountModel();
            m.CountList = new List<StepCount>();
            var response = await _accountOpeningRepository.StepsCounts(userId);
            foreach (var step in response)
            {
                m.CountList.Add(new StepCount
                {
                    Id = step.Id,
                    Title = step.Title,
                    TotalSteps = step.TotalSteps,
                    CompletedSteps = step.CompletedSteps,
                    Enabled = step.IsEnabled
                });
            }
            return ApiResponseWithData<CompletedStepsCountModel>.SuccessResponse(m);
        }

        //other
        public async Task DoesAnEntryExistsAgainstUserId(long userId)
        {
            var isExists = await _accountOpeningRepository.DoesAnEntryExistsAgainstUserId(userId);
            if (isExists)
                throw new ApiException("You have already selected a fund.");
        }
        //private

        public async Task<long> GetOnBoardingIdByUserId(long userId)
        {
            var accountOpeningId = await _accountOpeningRepository.GetAccountOpeningIdByUserId(userId);
            return accountOpeningId;
        }

        private async Task<long> GetAccountOpeningIdByUserId(long userId)
        {
            var accountOpeningId = await _accountOpeningRepository.GetAccountOpeningIdByUserId(userId);
            if (accountOpeningId < 1)
                throw new ApiException("Please select account type first.");
            return accountOpeningId;
        }

        public async Task<ApiResponseWithData<GetAccountOpeningIdResponseModel>> GetAccountOpeningIdByUserIdPublic(long userId)
        {
            var accountOpeningId = await _accountOpeningRepository.GetAccountOpeningIdByUserId(userId);
            if (accountOpeningId < 1)
                throw new ApiException("Please select account type first.");
            var response  =new GetAccountOpeningIdResponseModel { AccountOpeningId = accountOpeningId };


            return ApiResponseWithData<GetAccountOpeningIdResponseModel>.SuccessResponse(response); 
        }
        public async Task<ApiResponseWithData<GetAccountTypeResponseModel>>GetSavedAccountSelection(long userId)
        {
            var response = await _accountOpeningRepository.GetAccountSelection(userId);
            var selectedFundType = new GetAccountTypeResponseModel
            {
                AccountType = response.ACCOUNT_TYPE.Value
            };
            return ApiResponseWithData<GetAccountTypeResponseModel>.SuccessResponse(selectedFundType);
        }
        private async Task<GetAccountTypeResponseModel> GetSavedAccountSelectionPublic(long userId)
        {
            var response = await _accountOpeningRepository.GetAccountSelection(userId);
            var selectedFundType = new GetAccountTypeResponseModel
            {
                AccountType = response.ACCOUNT_TYPE.Value
            };
            return selectedFundType;
        }
        private async Task IsBasicInfoEnabled(long userId)
        {
            var enabled = await _accountOpeningRepository.IsBasicInfoEnabled(userId);
            if (!enabled)
                throw new ApiException("Please select transaction type first.");
        }
        private async Task IsPersonalDetailsEnabled(long userId)
        {
            var enabled = await _accountOpeningRepository.IsPersonalDetailsEnabled(userId);
            if (!enabled)
                throw new ApiException("Please enter Basic Information first.");
        }
        private async Task IsRiskProfileEnabled(long userId)
        {
            var enabled = await _accountOpeningRepository.IsRiskProfileEnabled(userId);
            if (!enabled)
                throw new ApiException("Please enter Personal Details first.");
        }
        private async Task IsRegulatoryKYCEnabled(long userId)
        {
            var enabled = await _accountOpeningRepository.IsRegulatoryKYCEnabled(userId);
            if (!enabled)
                throw new ApiException("Please enter Risk Profile first.");
        }
        private async Task IsUploadDocumentEnabled(long userId)
        {
            var enabled = await _accountOpeningRepository.IsUploadDocumentEnabled(userId);
            if (!enabled)
                throw new ApiException("Please enter Regulatory KYC first.");
        }
        //private async Task CheckOnBoardingStatus(long userId)
        //{
        //    var status = await _accountOpeningRepository.OnBoardingStatus(userId);
        //    var errorMsg = "Unable to proceed your request";
        //    if (status ==Convert.ToInt16(OnboardingApprovalStatus.Awaiting))
        //    {
        //        throw new ApiException($"{errorMsg}, your onboarding request is already awaiting for an approval");
        //    }
        //    else if (status == Convert.ToInt16(OnboardingApprovalStatus.Approved))
        //    {
        //        throw new ApiException($"{errorMsg}, your onboarding request is already approved");
        //    } 
        //    else if (status == Convert.ToInt16(OnboardingApprovalStatus.Rejected))
        //    {
        //        throw new ApiException($"{errorMsg}, your onboarding request was rejected");
        //    }
        //}
        private async Task<string?> CheckOnBoardingStatusAccounttype(long userId)
        {
            var status = await _accountOpeningRepository.OnBoardingStatus(userId);
            var errorMsg = "Unable to proceed your request";

            if (status == Convert.ToInt16(OnboardingApprovalStatus.Awaiting))
            {
                return $"{errorMsg}, your onboarding request is already awaiting for an approval";
            }
            else if (status == Convert.ToInt16(OnboardingApprovalStatus.Approved))
            {
                return $"{errorMsg}, your onboarding request is already approved";
            }
            else if (status == Convert.ToInt16(OnboardingApprovalStatus.Rejected))
            {
                return $"{errorMsg}, your onboarding request was rejected";
            }

            return null; //
        }



        public async Task<ApiResponseWithData<OnboardingStatusResponse>> OnBoardingStatusAccounttype(long userId)
        {
            var status = await _accountOpeningRepository.OnBoardingStatus(userId);

            if (status == Convert.ToInt16(OnboardingApprovalStatus.Awaiting))
            {
                var response = new OnboardingStatusResponse
                {
                    Status = "Awaiting",
                    Message = "Your onboarding request is awaiting approval"
                };

                return ApiResponseWithData<OnboardingStatusResponse>.SuccessResponse(response);
            }
            else if (status == Convert.ToInt16(OnboardingApprovalStatus.Approved))
            {
                var response = new OnboardingStatusResponse
                {
                    Status = "Approved",
                    Message = "Your onboarding request is already approved"
                };

                return ApiResponseWithData<OnboardingStatusResponse>.SuccessResponse(response);
            }
            else if (status == Convert.ToInt16(OnboardingApprovalStatus.Rejected))
            {
                var response = new OnboardingStatusResponse
                {
                    Status = "Rejected",
                    Message = "Your onboarding request was rejected"
                };

                return ApiResponseWithData<OnboardingStatusResponse>.SuccessResponse(response);
            }
            else if (status == Convert.ToInt16(OnboardingApprovalStatus.None))
            {
                var response = new OnboardingStatusResponse
                {
                    Status = "Incomplete",
                    Message = "Your onboarding request Its Not Submitted yet"
                };

                return ApiResponseWithData<OnboardingStatusResponse>.SuccessResponse(response);
            }

            return ApiResponseWithData<OnboardingStatusResponse>.SuccessResponse(new OnboardingStatusResponse
            {
                Status = "Incomplete",
                Message = "Your onboarding request Status Its Not listed."
            });
        }


    }
}