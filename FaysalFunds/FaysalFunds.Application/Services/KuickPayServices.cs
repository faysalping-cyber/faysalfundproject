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
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.TransactionAllowed;
using FaysalFunds.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public KuickPayServices(InvesmentFundRepository kuickPayRepository, IKpSlabRepository kpSlabRepository, ITransactionTypesGroupRepository transactionTypesGroupRepository,
            IFundFeaturePermissionRepository fundFeaturePermissionRepository,
            ITransactionFeatureRepository transactionFeatureRepository, IFamlFundRepository famlFundRepository, ITransactionReceiptDetailRepository transactionReceiptDetailRepository, IInvestmentInstructionRepository investmentinstructionRepository, IAccountOpeningRepository accountOpeningRepository, TransactionPinService transactionPinService)
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

        //public async Task<ApiResponseWithData<List<InvestmentInstructionsDTO>>> GetInvestmentInstructions()
        //{
        //    var instructions = await _investmentinstructionRepository.GetInvestmentMethods();

        //    if (instructions == null || !instructions.Any())
        //        throw new ApiException("No Investment Methods Found");

        //    var dtoList = instructions.Select(x => new InvestmentInstructionsDTO
        //    {
        //        Channel = x.CHANNEL,
        //        Title = x.TITLE,
        //        Steps = x.CONTENT
        //            .Split(';', StringSplitOptions.RemoveEmptyEntries)
        //            .Select(step => step.Trim())
        //            .ToList()
        //    }).ToList();

        //    return ApiResponseWithData<List<InvestmentInstructionsDTO>>.SuccessResponse(dtoList);
        //}

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

        //Select invested Funds


        public async Task<ApiResponseWithData<Dictionary<string, List<AlreadyInvestedFundsDTO>>>> SelectinvestedFunds(AccountOpeningRequestModel request)
        {
            // Step 1: Get all transactions for this user
            var accountDetails = await _transactionReceiptDetailRepository.GetByAccountID(request.UserId);

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
