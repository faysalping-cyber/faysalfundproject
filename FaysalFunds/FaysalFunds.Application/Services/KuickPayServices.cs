using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.DTOs.AccountOpening.BasicInformation;
using FaysalFunds.Application.DTOs.AccountOpening.RegulatoryKYC;
using FaysalFunds.Application.DTOs.AccountOpening.UploadDocument;
using FaysalFunds.Application.DTOs.TransactionAllowedDTO;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using FaysalFunds.Domain.DTOs.ExternalAPI;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using FaysalFunds.Domain.Interfaces;
//using FaysalFunds.Infrastructure.ExternalService;


namespace FaysalFunds.Application.Services
{
    public class KuickPayServices
    {
        private readonly InvesmentFundRepository _kuickPayRepository;
        private readonly IKpSlabRepository _kpSlabRepository;
        private readonly ITransactionTypesGroupRepository _transactionTypesGroupRepository;
        private readonly ITransactionFeatureRepository _transactionFeatureRepository;
        private readonly IFundFeaturePermissionRepository _fundFeaturePermissionRepository;
        private readonly IFamlFundRepository _famlFundRepository;
        private readonly ITransactionReceiptDetailRepository _transactionReceiptDetailRepository;
        private readonly IInvestmentInstructionRepository _investmentinstructionRepository;
        private readonly IAccountOpeningRepository _accountOpeningRepository;
        private readonly TransactionPinService _transactionPinService;
        private readonly IFamlInternalService _famlInternalService;
        private readonly IAccountRepository _accountRepository;

        public KuickPayServices(InvesmentFundRepository kuickPayRepository, IKpSlabRepository kpSlabRepository, ITransactionTypesGroupRepository transactionTypesGroupRepository,
            IFundFeaturePermissionRepository fundFeaturePermissionRepository,
            ITransactionFeatureRepository transactionFeatureRepository, IFamlFundRepository famlFundRepository, ITransactionReceiptDetailRepository transactionReceiptDetailRepository, IInvestmentInstructionRepository investmentinstructionRepository, IAccountOpeningRepository accountOpeningRepository, TransactionPinService transactionPinService, IFamlInternalService famlInternalService, IAccountRepository accountRepository)
        {

            _kuickPayRepository = kuickPayRepository;
            _kpSlabRepository = kpSlabRepository;
            _transactionTypesGroupRepository = transactionTypesGroupRepository;
            _fundFeaturePermissionRepository = fundFeaturePermissionRepository;
            _transactionFeatureRepository = transactionFeatureRepository;
            _famlFundRepository = famlFundRepository;
            _transactionReceiptDetailRepository = transactionReceiptDetailRepository;
            _investmentinstructionRepository = investmentinstructionRepository;
            _accountOpeningRepository = accountOpeningRepository;
            _transactionPinService = transactionPinService;
            _famlInternalService = famlInternalService;
            _accountRepository = accountRepository;
            //_famlInternalService = famlInternalService;

        }

        //Get Invetment Funds
        public async Task<ApiResponseWithData<InvestmentFundsDTO>> GetAllFunds()
        {
            var entities = await _kuickPayRepository.GetAllFunds();
            if (entities == null || !entities.Any())
                throw new ApiException("No Investment Fund found.");

            var lowList = new List<FundItem>();
            var mediumList = new List<FundItem>();
            var highList = new List<FundItem>();

            foreach (var item in entities)
            {
                var fund = new FundItem
                {
                    ID = item.ID,
                    FUNDNAME = item.FUNDNAME,
                    FUNDCATEGORY = item.FUNDCATEGORY,
                  
                    VIEWDETAIL = new ViewDetails
                    {
                        RISKPROFILE= item.RISKPROFILE,
                        GENDER = item.GENDER,
                        MONTHLYPROFILT = item.MONTHLYPROFILT,
                        FELPERCENTAGE = item.FELPERCENTAGE,
                        ISENABLE = item.ISENABLE
                    }
                };
                switch (item.RISKPROFILE?.Trim())
                {
                    case "Low":
                        lowList.Add(fund);
                        break;
                    case "Medium":
                        mediumList.Add(fund);
                        break;
                    case "High":
                        highList.Add(fund);
                        break;
                }
            }

            var groupedResult = new InvestmentFundsDTO
            {
                Low = lowList,
                Medium = mediumList,
                High = highList
            };

            return ApiResponseWithData<InvestmentFundsDTO>.SuccessResponse(groupedResult);
        }
        //Get Kuickpay Charges

        public async Task<ApiResponseWithData<List<KpSlabDTO>>> GetAllKuickPayCharges()
        {

            var entities = await _kpSlabRepository.GetAllKuickPayCharges();
            if (entities == null || !entities.Any())
            throw new ApiException("No KuickPay charges found..");

            var dtoList = entities.Select(kp => new KpSlabDTO
            {
                UPPER_LIMIT = kp.UPPER_LIMIT,
                LOWER_LIMIT = kp.LOWER_LIMIT,
                FEE_LIMIT = kp.FEE_LIMIT
            }).ToList();

            return ApiResponseWithData<List<KpSlabDTO>>.SuccessResponse(dtoList);

        }
        //TransactionTypesGroupDTO
        public async Task<ApiResponseWithData<List<TransactionTypesGroupDTO>>> GetAllTransactionTypes()
        {

            var entities = await _transactionTypesGroupRepository.GetTransactionTypes();
            if (entities == null || !entities.Any())
                throw new ApiException("No transaction Type Found..");

            var dtoList = entities.Select(tr => new TransactionTypesGroupDTO
            {
                ID = tr.ID,
                GROUP_NAME = tr.GROUP_NAME,
            }).ToList();

            return ApiResponseWithData<List<TransactionTypesGroupDTO>>.SuccessResponse(dtoList);

        }
        //Get All transaction Feature
        //public async Task<ApiResponseWithData<List<TransactionFeatureGroupDTO>>> GetAllTransaconFeature()
        //{
        //    var TFeature = await _transactionFeatureRepository.GetAllFeatures();

        //    if (TFeature == null || !TFeature.Any())
        //        throw new ApiException("No Transaction Data Found");

        //    // Map to DTO
        //    var dtoList = TFeature.Select(TF => new TransactionFeaturesDTO
        //    {
        //        ID = TF.ID,
        //        FEATURE_NAME = TF.FEATURE_NAME,
        //        FEATURE_GROUP = TF.FEATURE_GROUP,
        //        PAYMENT_MODE = TF.PAYMENT_MODE,
        //        ICON = TF.ICON 
        //    }).ToList();

        //    // Group by FEATURE_GROUP
        //    var groupedResult = dtoList
        //        .GroupBy(x => x.FEATURE_GROUP)
        //        .Select(g => new TransactionFeatureGroupDTO
        //        {
        //            TransactionGroupName = g.Key,
        //            TransactionFeatures = g.ToList()
        //        }).ToList();

        //    return ApiResponseWithData<List<TransactionFeatureGroupDTO>>.SuccessResponse(groupedResult);
        //}

        //public async Task<ApiResponseWithData<Dictionary<string, List<TransactionFeaturesDTO>>>> GetAllTransaconFeature()
        //{
        //    var features = await _transactionFeatureRepository.GetAllFeatures();

        //    if (features == null || !features.Any())
        //        throw new ApiException("No Transaction Data Found");

        //    // Map to DTO
        //    var dtoList = features.Select(f => new TransactionFeaturesDTO
        //    {
        //        ID = f.ID,
        //        FEATURE_NAME = f.FEATURE_NAME,
        //        FEATURE_GROUP = f.FEATURE_GROUP,
        //        PAYMENT_MODE = f.PAYMENT_MODE,
        //        ICON = f.ICON
        //    }).ToList();

        //    // Group by FEATURE_GROUP into Dictionary<string, List<TransactionFeaturesDTO>>
        //    var groupedDict = dtoList
        //        .GroupBy(x => x.FEATURE_GROUP)
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.ToList()
        //        );

        //    return ApiResponseWithData<Dictionary<string, List<TransactionFeaturesDTO>>>.SuccessResponse(groupedDict);
        //}
        public async Task<ApiResponseWithData<TransactionFeaturesGroupedDTO>> GetAllTransaconFeature()
        {
            var features = await _transactionFeatureRepository.GetAllFeatures();

            if (features == null || !features.Any())
                throw new ApiException("No Transaction Data Found");

            var investmentList = new List<TransactionFeaturesDTO>();
            var conversionList = new List<TransactionFeaturesDTO>();
            var withdrawalList = new List<TransactionFeaturesDTO>();

            foreach (var f in features)
            {
                var dto = new TransactionFeaturesDTO
                {
                    ID = f.ID,
                    FEATURE_NAME = f.FEATURE_NAME,
                    FEATURE_GROUP = f.FEATURE_GROUP,
                    PAYMENT_MODE = f.PAYMENT_MODE,
                    ICON = f.ICON
                };

                switch (f.FEATURE_GROUP?.Trim())
                {
                    case "Investment":
                        investmentList.Add(dto);
                        break;
                    case "Conversion":
                        conversionList.Add(dto);
                        break;
                    case "Withdrawal":
                        withdrawalList.Add(dto);
                        break;
                    default:
                        // Optional: handle unknown groups if needed
                        break;
                }
            }

            var groupedResult = new TransactionFeaturesGroupedDTO
            {
                Investment = investmentList,
                Conversion = conversionList,
                Withdrawal = withdrawalList
            };

            return ApiResponseWithData<TransactionFeaturesGroupedDTO>.SuccessResponse(groupedResult);
        }

        public async Task<ApiResponseWithData<TransactionFeaturesDTO>> GetTransactionFeatureById(TransactionID request)
        {
            var response = await _transactionFeatureRepository.GetTransactionFeatureById(request.TransactionFeatureID);
            if (response == null)
                return ApiResponseWithData<TransactionFeaturesDTO>.FailureResponse("Failed");
            var responseModel = new TransactionFeaturesDTO
            {
                ID = response.ID,
                FEATURE_NAME = response.FEATURE_NAME,
                FEATURE_GROUP = response.FEATURE_GROUP,
                PAYMENT_MODE = response.PAYMENT_MODE,
                ICON = response.ICON,

            };
            return ApiResponseWithData<TransactionFeaturesDTO>.SuccessResponse(responseModel);
        }

        //get Accountype by id
        public async Task<ApiResponseWithData<Fund>> GetActiveFundById(long UserId)
        {
            var accountTypeID = await _accountOpeningRepository.GetAccountSelection(UserId);

            var item = await _famlFundRepository.GetByActiveFundId((long) accountTypeID.ACCOUNT_TYPE);

            if (item == null)
                return ApiResponseWithData<Fund>.FailureResponse("No active fund available.");

            var fund = new Fund
            {
                Id = item.ID,
                Title = item.TITLE,
                AllTimeInvestmentLimit = item.ALLTIMEINVESTMENTLIMIT,
                AnnualInvestmentLimit = item.ANNUALINVESTMENTLIMIT,
                PerTransactionLimit = item.PERTRANSACTIONLIMIT,
                FirstTransactionMin = item.FIRST_TRANSACTION_MIN,
                SubsequentTransactionMin = item.SUBSEQUENT_TRANSACTION_MIN
            };

            return ApiResponseWithData<Fund>.SuccessResponse(fund);
        }

        //Get Investment Instruction

        public async Task<ApiResponseWithData<Dictionary<string, List<InvestmentInstructionsDTO>>>> GetInvestmentInstructions()
        {
            var instructions = await _investmentinstructionRepository.GetInvestmentMethods();

            if (instructions == null || !instructions.Any())
                throw new ApiException("No Investment Methods Found");

            // Create the grouped dictionary
            var groupedInstructions = instructions
                .GroupBy(x => x.CHANNEL?.Trim())
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(item => new InvestmentInstructionsDTO
                    {
                        Channel=item.CHANNEL,
                        Title = item.TITLE,
                        Steps = item.CONTENT
                            .Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(step => step.Trim())
                            .ToList()
                    }).ToList()
                );

            return ApiResponseWithData<Dictionary<string, List<InvestmentInstructionsDTO>>>.SuccessResponse(groupedInstructions);
        }

        //feature permission Yes or no case
        public async Task<ApiResponseWithData<FeaturePermissionResponse>> GetFeaturePermissions(FeaturePermissionRequestDTO model)
        {
            if (model == null || model.FundId <= 0)
            {
                return ApiResponseWithData<FeaturePermissionResponse>.FailureResponse("Invalid FundId");
            }

            if (model.TransactionFeatureId <= 0)
            {
                return ApiResponseWithData<FeaturePermissionResponse>.FailureResponse("Invalid TransactionFeatureId");
            }

            //Console.WriteLine($"Calling IsFundInTransactionFeature with FundId: {model.FundId}, TransactionFeatureId: {model.TransactionFeatureId}");
            var permission = await _fundFeaturePermissionRepository.IsFundInTransactionFeature(model.FundId, model.TransactionFeatureId);
            //Console.WriteLine($"Permission: {permission != null}, IsAllowed: {permission?.IS_ALLOWED}");

            if (permission == null)
            {
                return ApiResponseWithData<FeaturePermissionResponse>.FailureResponse("Permission not found for the specified fund and feature");
            }
            bool isAllowed = permission.IS_ALLOWED == "1";

            if (!isAllowed)
            {
                throw new ApiException("Transaction is not allowed for this Fund.");
            }

            var result = new FeaturePermissionResponse
            {
                IsAllowed = true
            };

            return ApiResponseWithData<FeaturePermissionResponse>.SuccessResponse(result, "Transaction is allowed for this Fund.");
        }

        public async Task<ApiResponseWithData<RaastAllowedOrNot>> RaastAllowedorNot(AccountOpeningRequestModel request)
        {
            var accountTypeID = await _accountOpeningRepository.GetAccountSelection(request.UserId);

            var accountTypeInfo = await _famlFundRepository.GetByActiveFundId((long)accountTypeID.ACCOUNT_TYPE);
            if(accountTypeInfo == null)
            {
                throw new ApiException("No Account Type Found");
            }
            bool isAllowed = accountTypeInfo?.TITLE == "Digital Sarmayakari Account";
            if (!isAllowed)
            {
                throw new ApiException("Raast is not allowed for this account type.");
            }
            var result = new RaastAllowedOrNot
            {
                IsAllowed = isAllowed
            };

         

            return ApiResponseWithData<RaastAllowedOrNot>.SuccessResponse(result, "Raast is allowed for this account type.");
        }


        //Api Calculation
        public async Task<ApiResponseWithData<CalculateKuickPayDTO>> CalculateKuickPay(CalculateKuickPayLoad payload)
        {
            var IsAlreadyInvested = await _transactionReceiptDetailRepository.GetByFolio(payload.FolioNumber);
            var accountTypeID = await _accountOpeningRepository.GetAccountSelection(payload.UserId);

            var item = await _famlFundRepository.GetByActiveFundId((long)accountTypeID.ACCOUNT_TYPE);
            var sahulatSarmyakari = await _famlFundRepository.GetByActiveFundId(item.ID);
            var ActivePaymentmode= await _transactionFeatureRepository.GetTransactionFeatureById(payload.PaymentMode);
            if (sahulatSarmyakari == null)
            {
                throw new ApiException("AccountType Id is Not null");
            }
            var fund = await _kuickPayRepository.GetByIdAsync(payload.FundID);
            if (fund == null)
                throw new ApiException("Fund not found.");

            string felString = fund.FELPERCENTAGE.Replace("%", "").Trim();
            if (!decimal.TryParse(felString, out decimal felPercentage))
                throw new ApiException("Invalid FEL percentage.");

            felPercentage = felPercentage / 100;

            int investedAmount = payload.Invested;
            int feldedu = (int)Math.Round(investedAmount * felPercentage);
            var kpSlabs = await _kpSlabRepository.GetAllKuickPayCharges();
            decimal kpCharges = 0;

            foreach (var slab in kpSlabs)
            {
                int lowerLimit = int.Parse(slab.LOWER_LIMIT);
                int upperLimit = int.Parse(slab.UPPER_LIMIT);

                if (investedAmount >= lowerLimit && investedAmount <= upperLimit)
                {
                    kpCharges = slab.FEE_LIMIT;
                    break;
                }
            }

            int totalAmount = investedAmount + (int)kpCharges;
            int amountInvested = investedAmount - feldedu;
            bool isMonthlyProfit = fund.MONTHLYPROFILT == "Y";
            string monthlyProfit = isMonthlyProfit ? "1" : "0";
            if (sahulatSarmyakari.TITLE == "Sahulat Sarmayakari Account"
                && (ActivePaymentmode.PAYMENT_MODE == "KuickPay" || ActivePaymentmode.PAYMENT_MODE == "IBFT"))

            {
                int minimumRequiredAmount;
                int higherTransactionLimit = sahulatSarmyakari.PERTRANSACTIONLIMIT;

                if (payload.Invested > sahulatSarmyakari.PERTRANSACTIONLIMIT)
                    throw new ApiException($"Investment cannot exceed {higherTransactionLimit} in a single transaction.");


                if (IsAlreadyInvested.Count == 0)
                {
                    // First transaction
                    minimumRequiredAmount = sahulatSarmyakari.FIRST_TRANSACTION_MIN;

                    if (payload.Invested < minimumRequiredAmount)
                        throw new ApiException($"Minimum first-time investment required is {minimumRequiredAmount}.");

                    if (payload.Invested > higherTransactionLimit)
                        throw new ApiException($"Maximum allowed per transaction is {higherTransactionLimit}.");

                }
                else
                {
                    // Subsequent transaction
                    minimumRequiredAmount = sahulatSarmyakari.SUBSEQUENT_TRANSACTION_MIN;

                    if (payload.Invested <= minimumRequiredAmount)
                        throw new ApiException($"Minimum subsequent investment required is {minimumRequiredAmount}.");

                    if (payload.Invested >= higherTransactionLimit)
                        throw new ApiException($"Maximum allowed per transaction is {higherTransactionLimit}.");

                    var today = DateTime.UtcNow;

                    var last365DaysTransactions = IsAlreadyInvested
                        .Where(x => (today - x.CREATEDON).TotalDays <= 365)
                        .ToList();

                    int totalInvestedIn365Days = last365DaysTransactions.Sum(x => x.AMOUNTINVESTED);
                    int totalInvestedAllTime = IsAlreadyInvested.Sum(x => x.AMOUNTINVESTED);

                    // Check 365-day limit
                    if (last365DaysTransactions.Any())
                    {
                        if (totalInvestedIn365Days + payload.Invested > sahulatSarmyakari.ANNUALINVESTMENTLIMIT)
                            throw new ApiException($"Annual investment limit of {sahulatSarmyakari.ANNUALINVESTMENTLIMIT} exceeded.");
                    }
                    else
                    {
                        if (totalInvestedAllTime + payload.Invested > sahulatSarmyakari.ALLTIMEINVESTMENTLIMIT)
                            throw new ApiException($"All-time investment limit of {sahulatSarmyakari.ALLTIMEINVESTMENTLIMIT} exceeded.");
                    }

                }
                switch (ActivePaymentmode.PAYMENT_MODE)
                {
                    case "KuickPay":
                        return ApiResponseWithData<CalculateKuickPayDTO>.SuccessResponse(new CalculateKuickPayDTO
                        {
                            FundName = fund.FUNDNAME,
                            FolioNumber = payload.FolioNumber,
                            Invested = investedAmount,
                            KPCharges = kpCharges.ToString(),
                            TotalAmount = totalAmount,
                            FelCharges = feldedu.ToString(),
                            AmountInvested = amountInvested,
                            MonthlyProfit = monthlyProfit
                        });

                    case "IBFT":
                        var IsAccounTtitleMatch = await _transactionReceiptDetailRepository.GetByFolio(payload.FolioNumber);

                        return ApiResponseWithData<CalculateKuickPayDTO>.SuccessResponse(new CalculateKuickPayDTO
                        {
                            FundName = fund.FUNDNAME,
                            FolioNumber = payload.FolioNumber,
                            Invested = investedAmount,
                            TotalAmount = totalAmount,
                            FelCharges = feldedu.ToString(),
                            AmountInvested = amountInvested,
                            MonthlyProfit = monthlyProfit
                  
                        });

                    default:
                        throw new ApiException($"Unsupported PaymentMode: {ActivePaymentmode.PAYMENT_MODE}");
                }

            }

            else if (sahulatSarmyakari.TITLE == "Digital Sarmayakari Account" && (ActivePaymentmode.PAYMENT_MODE == "KuickPay" || ActivePaymentmode.PAYMENT_MODE == "IBFT"))
            {

                switch (ActivePaymentmode.PAYMENT_MODE)
                {
                    case "KuickPay":
                        return ApiResponseWithData<CalculateKuickPayDTO>.SuccessResponse(new CalculateKuickPayDTO
                        {
                            FundName = fund.FUNDNAME,
                            FolioNumber = payload.FolioNumber,
                            Invested = investedAmount,
                            KPCharges = kpCharges.ToString(),
                            TotalAmount = totalAmount,
                            FelCharges = feldedu.ToString(),
                            AmountInvested = amountInvested,
                            MonthlyProfit = monthlyProfit
                        });

                    case "IBFT":
                        var IsAccounTtitleMatch = await _transactionReceiptDetailRepository.GetByFolio(payload.FolioNumber);

                        return ApiResponseWithData<CalculateKuickPayDTO>.SuccessResponse(new CalculateKuickPayDTO
                        {
                            FundName = fund.FUNDNAME,
                            FolioNumber = payload.FolioNumber,
                            Invested = investedAmount,
                            TotalAmount = totalAmount,
                            FelCharges = feldedu.ToString(),
                            AmountInvested = amountInvested,
                            MonthlyProfit = monthlyProfit

                        });

                    default:
                        throw new ApiException($"Unsupported PaymentMode: {ActivePaymentmode.PAYMENT_MODE}");
                }

            }
            else
            {
                throw new ApiException("account title or payment mode its Not match");
            }

        }


        public async Task<ApiResponseWithData<KuickPayReceiptDetailsDTO>> SaveKuickpayReceiptDetail(KuickPayReceiptPayload payload)
        {
            // Step 1: Calculate KuickPay values
            await _transactionPinService.IsTpinGenerated(payload.UserId);
            await _transactionPinService.VerifyTransactionPin(new() { AccountOpeningId = payload.UserId, Pin = payload.Pin });
            var calculated = await CalculateKuickPay(new CalculateKuickPayLoad
            {
                FolioNumber = payload.FolioNumber,
                FundID = payload.FundID,
                UserId = payload.UserId,
                Invested = payload.Invested,
                PaymentMode = payload.PaymentMode
            });
            // Step 2: Prepare the entity to save
            var ActivePaymentmode = await _transactionFeatureRepository.GetTransactionFeatureById(payload.PaymentMode);

            var newReceipt = new TransactionReceiptDetails
            {
                FOLIONUMBER = payload.FolioNumber,
                FUNDNAME = calculated.Data.FundName,
                KUICKPAYCHARGES = int.Parse(calculated.Data.KPCharges),
                FELCHARGES = int.Parse(calculated.Data.FelCharges),
                TOTALAMOUNT = calculated.Data.TotalAmount,
                MONTHLYPROFIT = payload.MonthlyProfit == 1 ? "Enable" : "Disable",
                AMOUNTINVESTED = calculated.Data.AmountInvested,
                KUICKPAYID = payload?.kuickPayID,
                PAYMENTMODE = payload.PaymentMode,
                TRANSACTIONTYPE = ActivePaymentmode.FEATURE_GROUP,
                ACKNOWLEDGE = payload.ACKNOWLEDGE,
                FUNDID = payload.FundID,
                ACCOUNTID =payload.UserId,
                DATETIME = DateTime.Now,
                CREATEDON = DateTime.Now,
            };

            // Step 3: Save the data
            var added = await _transactionReceiptDetailRepository.SaveKuickPayReceipt(newReceipt);

            if (!added)
                return ApiResponseWithData<KuickPayReceiptDetailsDTO>.FailureResponse("Failed to save.");

            // Step 4: Map saved entity to DTO

            var responseDto = new KuickPayReceiptDetailsDTO
            {
                TransactionID = $"FaysalFund{newReceipt.ID}",
                FolioNumber = newReceipt.FOLIONUMBER,
                FundName = newReceipt.FUNDNAME,
                KuickPayCharges = newReceipt.KUICKPAYCHARGES,
                FelCharges = newReceipt.FELCHARGES,
                TotalAmount = newReceipt.TOTALAMOUNT,
                MonthlyProfit = newReceipt.MONTHLYPROFIT,
                AmountInvested = newReceipt.AMOUNTINVESTED,
                KuickPayId = newReceipt.KUICKPAYID,
                PaymentMode = ActivePaymentmode.PAYMENT_MODE,
                TransactionType = ActivePaymentmode.FEATURE_GROUP,
                ACKNOWLEDGE = payload.ACKNOWLEDGE,
                DateTime = newReceipt.DATETIME,
                CreatedOn = newReceipt.CREATEDON,
       
            };

            return ApiResponseWithData<KuickPayReceiptDetailsDTO>.SuccessResponse(responseDto, "Saved successfully.");
        }


        //save IBFT and rerurn DTO
        public async Task<ApiResponseWithData<IBFTReceiptDetailDTO>> SaveIBFTReceiptDetail(IBFTReceiptPayload payload)
        {
            await _transactionPinService.IsTpinGenerated(payload.UserId);
            await _transactionPinService.VerifyTransactionPin(new() { AccountOpeningId = payload.UserId, Pin = payload.Pin });
            // Step 1: Calculate KuickPay values
            var calculated = await CalculateKuickPay(new CalculateKuickPayLoad
            {
                FolioNumber = payload.FolioNumber,
                FundID = payload.FundID,
                UserId = payload.UserId,
                Invested = payload.Invested,
                PaymentMode = payload.PaymentMode
            });
            // Step 2: Prepare the entity to save
            var ActivePaymentmode = await _transactionFeatureRepository.GetTransactionFeatureById(payload.PaymentMode);

            var newReceipt = new TransactionReceiptDetails
            {
                FOLIONUMBER = payload.FolioNumber,
                FUNDNAME = calculated.Data.FundName,
                FELCHARGES = int.Parse(calculated.Data.FelCharges),
                TOTALAMOUNT = calculated.Data.TotalAmount,
                MONTHLYPROFIT = payload.MonthlyProfit == 1 ? "Enable" : "Disable",
                AMOUNTINVESTED = calculated.Data.AmountInvested,
                PAYMENTMODE = payload.PaymentMode,
                TRANSACTIONTYPE = ActivePaymentmode.FEATURE_GROUP,
                BANK_NAME = payload?.BankName,
                IBAN = payload?.IBAN,
                TRANSACTION_PROOF_PATH = payload?.TransactionProof,
                IS_EXISTING_ACCOUNT = payload?.IsExistingBank ?? 0,
                ACKNOWLEDGE = payload.ACKNOWLEDGE,
                FUNDID = payload.FundID,
                ACCOUNTID = payload.UserId,
                DATETIME = DateTime.Now,
                CREATEDON = DateTime.Now,
            };

            // Step 3: Save the data
            var added = await _transactionReceiptDetailRepository.SaveIBFTReceipt(newReceipt);

            if (!added)
                return ApiResponseWithData<IBFTReceiptDetailDTO>.FailureResponse("Failed to save.");

            // Step 4: Map saved entity to  

            var responseDto = new IBFTReceiptDetailDTO
            {
                TransactionID = $"FaysalFund{newReceipt.ID}",
                FolioNumber = newReceipt.FOLIONUMBER,
                FundName = newReceipt.FUNDNAME,
                FelCharges = newReceipt.FELCHARGES,
                TotalAmount = newReceipt.TOTALAMOUNT,
                MonthlyProfit = newReceipt.MONTHLYPROFIT,
                AmountInvested = newReceipt.AMOUNTINVESTED,
                PaymentMode = ActivePaymentmode.PAYMENT_MODE,
                TransactionType = ActivePaymentmode.FEATURE_GROUP,
                BankName = newReceipt.BANK_NAME,
                Iban = newReceipt.IBAN,
                TransactionProofPath = newReceipt.TRANSACTION_PROOF_PATH,
                ACKNOWLEDGE = payload.ACKNOWLEDGE,
                DateTime = newReceipt.DATETIME,
                CreatedOn = newReceipt.CREATEDON,
                IsExistingAccount = newReceipt.IS_EXISTING_ACCOUNT,
                // Add other properties if needed
            };

            return ApiResponseWithData<IBFTReceiptDetailDTO>.SuccessResponse(responseDto, "Saved successfully.");
        }

        

        public async Task<ApiResponseWithData<CalculateReceiptDTO>> CalculateConversionDetail(CaculateReceiptPayload payload)
        {
            
            
            // Step 2: Get account info
            var account = await _accountRepository.GetAccountByAccountId(payload.UserId);
            if (account == null)
                throw new ApiException("User account not found.");

            // Step 3: Fetch balance list from internal API
            var bankResponse = await _famlInternalService.CheckCustomerBalance(new CheckBalanceRequestModel
            {
                Folio = payload.FolioNumber,
                Cnic = account.CNIC,
                PhoneNo = account.PHONE_NO
            });

            if (bankResponse?.Data?.CheckBalanceList == null || !bankResponse.Data.CheckBalanceList.Any())
                throw new ApiException("Bank API did not return any balance records.");

            var folioBalances = bankResponse.Data.CheckBalanceList
                .Where(x => x.FolioNo == payload.FolioNumber)
                .ToList();

            if (!folioBalances.Any())
                throw new ApiException($"No balance records found for folio {payload.FolioNumber}.");

            // Step 4: Get old fund balance and name
            decimal oldFundBalance = 0;
            string oldFundName = "";
            foreach (var fund in folioBalances)
            {
                if (int.TryParse(fund.FUNDID, out var fundId) && fundId == payload.OldFundId)
                {
                    oldFundBalance = fund.BalanceAmount;
                    oldFundName = fund.FundName;
                    break;
                }
            }

            if (oldFundBalance <= 0)
                throw new ApiException($"No balance found for the selected fund (ID: {payload.OldFundId}).");

            // Step 5: Pending total
            var pendingAmount = await _transactionReceiptDetailRepository
                .GetPendingTotalAmount(payload.OldFundId, payload.FolioNumber);

            // Step 6: Calculate available balance
            var availableBalance = oldFundBalance - pendingAmount;

            if (availableBalance <= 0)
                throw new ApiException($"All available balance is already pending for conversion.");

            // ✅ Step 7: If CheckConvertAll = true, use full available balance as ConversionAmount
            decimal conversionAmount = payload.CheckConvertAll
                ? availableBalance
                : payload.ConversionAmount;

            // Validate again (in case user-provided amount > available)
            if (conversionAmount > availableBalance)
                throw new ApiException($"Insufficient balance. Available: {availableBalance}, Requested: {conversionAmount}");

            // Step 8: Get new fund info
            var newFund = await _kuickPayRepository.GetByIdAsync(payload.NewFundId)
                          ?? throw new ApiException("Destination fund not found.");

            string newFundName = newFund.FUNDNAME;

            // Step 9: FEL Charges
            string felString = newFund.FELPERCENTAGE.Replace("%", "").Trim();
            if (!decimal.TryParse(felString, out decimal felPercentage))
                throw new ApiException("Invalid FEL percentage format.");

            felPercentage /= 100;
            decimal felCharges = Math.Round(conversionAmount * felPercentage, 2);

            // Step 10: Calculate total amount (after FEL)
            decimal totalAmount = conversionAmount - felCharges;

            // Step 11: Prepare DTO
            var responseDto = new CalculateReceiptDTO
            {
                FolioNumber = payload.FolioNumber,
                FundFrom = oldFundName,
                FundTo = newFundName,
                AmountConverted = conversionAmount,
                FELCharges = felCharges,
                CGTApplicable = "Applicable", // default for now
                TotalAmount = totalAmount,
                AvailableBalanceAtTransaction = availableBalance,

                MonthlyProfit = 1
            };

            return ApiResponseWithData<CalculateReceiptDTO>.SuccessResponse(
                responseDto,
                "Conversion detail calculated successfully."
            );
        }

        public async Task<ApiResponseWithData<ConversionReceiptDetailDTO>> SaveConversionReceiptDetail(ConversionReceiptPayload payload)
        {
            // ✅ Step 1: Verify T-PIN first
            //await _transactionPinService.IsTpinGenerated(payload.UserId);
            //await _transactionPinService.VerifyTransactionPin(new()
            //{
            //    AccountOpeningId = payload.UserId,
            //    Pin = payload.Pin
            //});

            // ✅ Step 2: Call CalculateConversionDetail to ensure valid + recalculated data
            var calcPayload = new CaculateReceiptPayload
            {
                UserId = payload.UserId,
                FolioNumber = payload.FolioNumber,
                OldFundId = payload.OldFundId,
                NewFundId = payload.NewFundId,
                ConversionAmount = payload.ConversionAmount,
                PAYMENTMODE = payload.PAYMENTMODE,
                CheckConvertAll = payload.CheckConvertAll
            };

            var calcResponse = await CalculateConversionDetail(calcPayload);
            var data = calcResponse.Data;

            if (data == null)
                throw new ApiException("Failed to calculate conversion details.");

            // ✅ Step 3: Get readable Fund Names
            var newFund = await _kuickPayRepository.GetByIdAsync(payload.NewFundId);

      
            if (newFund == null)
                throw new ApiException($"Fund not found for NewFundId {payload.NewFundId}");
            var ActivePaymentmode = await _transactionFeatureRepository.GetTransactionFeatureById(payload.PAYMENTMODE);

            // ✅ Step 4: Prepare entity for saving
            var entity = new TransactionReceiptDetails
            {
                ACCOUNTID = payload.UserId,
                FOLIONUMBER = data.FolioNumber,
                OLD_FUND_ID = payload.OldFundId,
                NEW_FUND_ID = payload.NewFundId,
                CONVERSION_AMOUNT = (int)data.AmountConverted,
                FELCHARGES = data.FELCharges,
                TOTALAMOUNT = data.TotalAmount,
                MONTHLYPROFIT = data.MonthlyProfit == 1 ? "Enable" : "Disable",
                AVAIL_BALANCE_AT_TRANSACTION = (int)data.AvailableBalanceAtTransaction,
                PAYMENTMODE = payload.PAYMENTMODE,
                ACKNOWLEDGE = payload.ACKNOWLEDGE,
                TRANSACTIONTYPE = ActivePaymentmode.FEATURE_GROUP,
                FUNDNAME =data.FundFrom,
                FUNDID = payload.OldFundId,
                STATUS = 1,
                CREATEDON = DateTime.Now,
                DATETIME = DateTime.Now
            };

            var saved = await _transactionReceiptDetailRepository.SaveConversionReceipt(entity);
            if (!saved)
                throw new ApiException("Failed to save conversion receipt.");

            // ✅ Step 5: Prepare Response DTO
            var response = new ConversionReceiptDetailDTO
            {
                TransactionId = $"FaysalFundConv{entity.ID}",
                TransactionType = ActivePaymentmode.FEATURE_GROUP,
                FolioNumber = data.FolioNumber,
                FundFrom = data.FundFrom,
                FundTo = data.FundTo,
                AmountConverted = data.AmountConverted,
                FELCharges = data.FELCharges,
                MonthlyProfit = data.MonthlyProfit,
                TotalAmount = data.TotalAmount,
                CreatedOn = entity.CREATEDON
            };

            return ApiResponseWithData<ConversionReceiptDetailDTO>.SuccessResponse(response, "Conversion saved successfully.");
        }


        public async Task<ApiResponseWithData<RedemptionReceiptDetailDTO>> SaveRedemptionReceiptDetail(RedemptionReceiptPayload payload)
        {
            // Step 1: Validate T-PIN
            await _transactionPinService.IsTpinGenerated(payload.UserId);
            await _transactionPinService.VerifyTransactionPin(new() { AccountOpeningId = payload.UserId, Pin = payload.Pin });

            // Step 2: Get Bank API balance
            var bankResponse = await _famlInternalService.CheckCustomerBalance(new CheckBalanceRequestModel
            {
                Folio = payload.FolioNumber,
                Cnic = null,       // optional
                PhoneNo = null     // optional
            });
            if (bankResponse == null)
                return ApiResponseWithData<RedemptionReceiptDetailDTO>.FailureResponse("Unable to fetch bank balance.");

            var bankBalance = 1500;
                //(decimal)(bankResponse.Data.CheckBalanceList
                //.FirstOrDefault(x => x.FolioNo == payload.FolioNumber)?.BalanceAmount ?? 0);

            // Step 3: Get pending total (conversion + redemption)
            var pendingAmount = await _transactionReceiptDetailRepository.GetPendingTotalAmount(payload.FolioNumber, payload.FundId);

            var effectiveBalance = bankBalance - pendingAmount;

            if (payload.RedemptionAmount > effectiveBalance)
                return ApiResponseWithData<RedemptionReceiptDetailDTO>.FailureResponse(
                    $"Insufficient balance. You can only redeem up to {effectiveBalance}."
                );

            // Step 4: Prepare entity
            var newReceipt = new TransactionReceiptDetails
            {
                FOLIONUMBER = payload.FolioNumber,
                FUNDID = payload.FundId,
                PAYMENTMODE = payload.PAYMENTMODE,

                REDEMPTION_AMOUNT = (int)Math.Truncate(payload.RedemptionAmount),
                AVAIL_BALANCE_AT_TRANSACTION = bankBalance,
                STATUS = 1, // Pending
                CREATEDON = DateTime.Now,
            };

            var added = await _transactionReceiptDetailRepository.SaveRedemptionReceipt(newReceipt);

            if (!added)
                return ApiResponseWithData<RedemptionReceiptDetailDTO>.FailureResponse("Failed to save redemption.");

            // Step 5: Return response
            var responseDto = new RedemptionReceiptDetailDTO
            {
                TransactionId = $"FaysalFundRed{newReceipt.ID}",
                FolioNumber = newReceipt.FOLIONUMBER,
                RedemptionAmount = newReceipt.REDEMPTION_AMOUNT,
                AvailableBalanceAtTransaction = newReceipt.AVAIL_BALANCE_AT_TRANSACTION,
                Status = "Pending",
                CreatedOn = newReceipt.CREATEDON
            };

            return ApiResponseWithData<RedemptionReceiptDetailDTO>.SuccessResponse(responseDto, "Redemption saved successfully.");
        }


        //Select invested Funds


        public async Task<ApiResponseWithData<Dictionary<string, List<AlreadyInvestedFundsDTO>>>> SelectinvestedFunds(AlreadyInvestedFundspayload request)
        {
            // Step 1: Get all transactions for this user
            var accountDetails = await _transactionReceiptDetailRepository.GetByAccountID(request.UserId,request.FolioNo);

            if (accountDetails == null || !accountDetails.Any())
            {
                throw new ApiException("No records found.");
            }

            var selectedFundsList = new List<AlreadyInvestedFundsDTO>();

            foreach (var transaction in accountDetails)
            {
                var fund = await _kuickPayRepository.GetByIdAsync((long)transaction.FUNDID);
                if (fund != null)
                {
                    selectedFundsList.Add(new AlreadyInvestedFundsDTO
                    {
                        FundID = fund.ID,
                        FundName = fund.FUNDNAME,
                        FundCategory = fund.FUNDCATEGORY,
                        MonthlyProfit = fund.MONTHLYPROFILT,
                        // These values are not in InvestmentFunds, adjust as needed
                        TotalAmount = transaction.TOTALAMOUNT,
                        RiskProfile = fund.RISKPROFILE,
                        FolioNo = transaction.FOLIONUMBER,

                    });
                }
            }
            var AlreadyInvestedFunds = new Dictionary<string, List<AlreadyInvestedFundsDTO>>
                 {
                  { "AlreadyInvestedFunds", selectedFundsList }
                  };
            return ApiResponseWithData< Dictionary<string, List<AlreadyInvestedFundsDTO>>>.SuccessResponse(AlreadyInvestedFunds);
        }


    }

}
