using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.Services;
using FaysalFunds.Common;
using FaysalFunds.Domain.Entities;
using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Linq.Expressions;
using static FaysalFunds.Application.DTOs.AccountOpening.AccountOpeningDTO;

namespace FaysalFunds.Persistence.Repositories
{
    public class AccountOpeningRepository : BaseRepository<AccountOpening>, IAccountOpeningRepository
    {
        private readonly DbSet<AccountOpening> _accountOpeningSet;
        private readonly Settings _settings;
        private readonly DropdownService _dropdownService;

        public AccountOpeningRepository(ApplicationDbContext dbContext, IOptions<Settings> options) : base(dbContext)
        {
            _accountOpeningSet = dbContext.Set<AccountOpening>();
            _settings = options.Value;
        }
        public async Task<bool> AddAccountSelection(AccountOpening model)
        {
            if (model.ID==0)
            {
                await AddAsync(model);
            }
            else
            {
                AttachAndMarkModified(model,
                x => x.ACCOUNT_TYPE
            );
            }
            
            return await SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveBasicInformationStep1(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.TITLE_FULL_NAME,
                x => x.FATHERORHUSBANDNAME,
                x => x.MOTHER_NAME,
                x => x.PRINCIPLE_CNIC,
                x => x.CNIC_ISSUANCE_DATE,
                x => x.PRINCIPLE_CNIC_EXP_DATE,
                x => x.PRINCIPLE_CNIC_EXP_LIFE_TIME,
                x => x.BASICINFO_STEP1
            );
            return await SaveChangesAsync() > 0;
        }


        public async Task<bool> SaveBasicInformationStep2(AccountOpening model)
        {
            // Remove UPLOADCZ_50 
            AttachAndMarkModified(model,
                x => x.DATE_OF_BIRTH,
                x => x.PLACE_OF_BIRTH,
                x => x.GENDER,
                x => x.RESIDENTIAL_STATUS,
                x => x.ZAKAT_STATUS,
                x => x.REFERRALCODE,
                x => x.BASICINFO_STEP2
            );

            var efResult = await SaveChangesAsync();

            // Ab sirf UPLOADCZ_50 ko raw SQL se update karte hain
            using var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        UPDATE ACCOUNTOPENING SET 
            UPLOADCZ_50 = :p_uploadcz50
        WHERE ID = :p_id";

            var uploadParam = new OracleParameter(":p_uploadcz50", OracleDbType.Blob)
            {
                Value = model.UPLOADCZ_50 ?? (object)DBNull.Value,
                Direction = ParameterDirection.Input
            };
            command.Parameters.Add(uploadParam);

            command.Parameters.Add(new OracleParameter(":p_id", OracleDbType.Int64)
            {
                Value = model.ID,
                Direction = ParameterDirection.Input
            });

            var rowsAffected = await command.ExecuteNonQueryAsync();

            return efResult + rowsAffected > 0;
        }


        public async Task<bool> SaveContactDetail(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.PERMANENT_ADDRESS,
                x => x.PERMANENT_COUNTRY,
                x => x.PERMANENT_CITY,
                x => x.IS_MAILING_SAME_AS_PERMANENT_ADDRESS,
                x => x.MAILING_ADDRESS_1,
                x => x.MAILING_COUNTRY,
                x => x.MAILING_CITY,
                x => x.EMAIL,
                x => x.MOB_NO_COUNTRY_CODE,
                x => x.MOBILE_NUMBER,
                x => x.MOBILE_NUMBER_OWNERSHIP,
                x => x.MOBILE_OWNERSHIP_PROOF,
                x => x.PERSONALDETAILS_STEP1
            );
            return await SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveNextToKinInformation(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.NEXTOFKIN_NAME,
                x => x.NEXTOFKIN_NO_COUNTRY_CODE,
                x => x.NEXTOFKIN_MOB_NUMBER,
                x => x.PERSONALDETAILS_STEP2

            );
            return await SaveChangesAsync() > 0;
        }
      
        public async Task<bool> SaveBankDetails(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.TYPE_OF_ACCOUNT,
                x => x.BANK_NAME,
                x => x.IBAN_NO,
                x => x.WALLET_NAME,
                x => x.WALLET_NO,
                x => x.DIVIDEND_PAYOUT,
                x => x.PERSONALDETAILS_STEP3

            );
            return await SaveChangesAsync() > 0;
        }
        public async Task<bool> SaveRiskProfileDropdownValues(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.AGE,
                x => x.MARITAL_STATUS,
                x => x.NO_OF_DEPENDENTS,
                x => x.RISKPROFILE_OCCUPATION,
                x => x.EDUCATION,
                x => x.RISK_APPETITE,
                x => x.INVESTMENT_OBJECTIVE,
                x => x.INVESTMENT_HORIZON,
                x => x.INVESTMENT_KNOWLEDGE,
                x => x.FINANCIALPOSITION,
                x => x.RISK_PROFILE_DECLARATION,
                x => x.RISKPROFILE_STEP
            );
            var data = model;
            return await SaveChangesAsync() > 0;
        }
        public async Task<bool> SaveRegulatoryKYC1(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.OCCUPATION,
                x => x.PROFESSION,
                x => x.SOURCE_OF_INCOME,
                x => x.NAME_OF_EMPLOYER_OR_BUSINESS,
                x => x.GROSS_ANNUAL_INCOME,
                x => x.EXPECTED_MONTHLY_INVESTEMENTAMOUNT,
                x => x.EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION,
                x => x.EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION,
                x => x.REGULATORYKYC_STEP1



            );
            return await SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveRegulatoryKYC2(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.FINANCIAL_INSTITUUTION_REFUSAL,
                x => x.ULTIMATE_BENEFICIARY,
                x => x.HOLDING_SENIOR_POSITION,
                x => x.INTERNATIONAL_RELATION_OR_PEP,
                x => x.DEAL_WITH_HIGH_VALUE_ITEMS,
                x => x.FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS,
                x => x.TRUE_INFORMATION_DECLARATION,
                x => x.REGULATORYKYC_STEP2
            );
            return await SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveFATCA(AccountOpening model)
        {
            // EF Core se non-BLOB fields update kar rahe hain
            AttachAndMarkModified(model,
                x => x.COUNTRY_OF_RESIDENT,
                x => x.IS_US_CITIZEN_RESIDENT_HAVEGREENCARD,
                x => x.US_BORN,
                x => x.INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA,
                x => x.HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS,
                x => x.HAVE_US_TELEPHONE_NUMBER,
                x => x.US_TAXPAYER_IDENTIFICATION_NUMBER,
                x => x.FATCA_DECLARATION,
                x => x.NON_US_PERSON_DECLARATION,
                x => x.REGULATORYKYC_STEP3
            );

            var efResult = await SaveChangesAsync();

            using var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
UPDATE ACCOUNTOPENING SET 
    W9_FORM_UPLOAD = :p_w9formupload
WHERE ID = :p_id";

            var uploadParam = new OracleParameter(":p_w9formupload", OracleDbType.Blob)
            {
                Value = model.W9_FORM_UPLOAD ?? (object)DBNull.Value,
                Direction = ParameterDirection.Input
            };
            command.Parameters.Add(uploadParam);

            command.Parameters.Add(new OracleParameter(":p_id", OracleDbType.Int64)
            {
                Value = model.ID,
                Direction = ParameterDirection.Input
            });

            var rowsAffected = await command.ExecuteNonQueryAsync();

            return efResult + rowsAffected > 0;
        }


        //public async Task<bool> SaveFATCA(AccountOpening model)
        //{
        //    AttachAndMarkModified(model,
        //        x => x.COUNTRY_OF_RESIDENT,
        //        x => x.IS_US_CITIZEN_RESIDENT_HAVEGREENCARD,
        //        x => x.US_BORN,
        //        x => x.INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA,
        //        x => x.HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS,
        //        x => x.HAVE_US_TELEPHONE_NUMBER,
        //        x => x.US_TAXPAYER_IDENTIFICATION_NUMBER,
        //        x => x.W9_FORM_UPLOAD,
        //        x => x.FATCA_DECLARATION,
        //        x => x.NON_US_PERSON_DECLARATION,
        //        x => x.REGULATORYKYC_STEP3


        //    );
        //    return await SaveChangesAsync() > 0;
        //}

        public async Task<bool> SaveCRS(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.TAX_RESIDENT_COUNTRY,
                x => x.TIN_NO,
                x => x.REASON_FOR_NO_TIN_NUMBER,
                x => x.HAVE_TIN,
                x => x.CRS_DECLARATION,
                x => x.REGULATORYKYC_STEP4

            );
            return await SaveChangesAsync() > 0;
        }
        public async Task<bool> PostOnboardingProfile(AccountOpening model)
        {
            AttachAndMarkModified(model,
                x => x.STATUS

            );
            return await SaveChangesAsync() > 0;
        }
        //public async Task<bool> UploadProfileDocuments(AccountOpening model)
        //{
        //    AttachAndMarkModified(model,
        //        x => x.CNIC_UPLOAD_FRONT,
        //        x => x.CNIC_UPLOAD_BACK,
        //        x => x.LIVEPHOTE_OR_SELFIE,
        //        x => x.PROOF_OF_SOURCE_OF_INCOME,
        //        x => x.PAST_ONE_YEAR_BANKSTATEMENT,
        //        x => x.UPLOADDOCUMENT_STEP,
        //        x => x.FAMILY_MEMBER,
        //        x => x.INTERNATIONAL_NUMBER,
        //        x => x.COMPANY_REGISTER
        //        //x => x.STATUS

        //    );
        //    var data = model;
        //    return await SaveChangesAsync() > 0;
        //}

        public async Task<bool> UploadProfileDocuments(AccountOpening model)
        {
            // Attach non-blob properties partially (example: UPLOADDOCUMENT_STEP)
            _accountOpeningSet.Attach(model);
            _context.Entry(model).Property(x => x.UPLOADDOCUMENT_STEP).IsModified = true;
            // Add other non-blob properties here as needed

            var efResult = await _context.SaveChangesAsync();

            // Update blobs via raw SQL to avoid ORA-01460 error
            using var connection = _context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        UPDATE ACCOUNTOPENING SET 
            CNIC_UPLOAD_FRONT = :p_cnic_front,
            CNIC_UPLOAD_BACK = :p_cnic_back,
            LIVEPHOTE_OR_SELFIE = :p_live_photo,
            PROOF_OF_SOURCE_OF_INCOME = :p_proof_income,
            PAST_ONE_YEAR_BANKSTATEMENT = :p_bank_statement,
            FAMILY_MEMBER = :p_family_member,
            INTERNATIONAL_NUMBER = :p_intl_number,
            COMPANY_REGISTER = :p_company_register
        WHERE ID = :p_id";

            void AddBlobParameter(string paramName, byte[]? value)
            {
                var param = new OracleParameter(paramName, OracleDbType.Blob)
                {
                    Value = value ?? (object)DBNull.Value,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(param);
            }

            AddBlobParameter(":p_cnic_front", model.CNIC_UPLOAD_FRONT);
            AddBlobParameter(":p_cnic_back", model.CNIC_UPLOAD_BACK);
            AddBlobParameter(":p_live_photo", model.LIVEPHOTE_OR_SELFIE);
            AddBlobParameter(":p_proof_income", model.PROOF_OF_SOURCE_OF_INCOME);
            AddBlobParameter(":p_bank_statement", model.PAST_ONE_YEAR_BANKSTATEMENT);
            AddBlobParameter(":p_family_member", model.FAMILY_MEMBER);
            AddBlobParameter(":p_intl_number", model.INTERNATIONAL_NUMBER);
            AddBlobParameter(":p_company_register", model.COMPANY_REGISTER);

            command.Parameters.Add(new OracleParameter(":p_id", OracleDbType.Int64)
            {
                Value = model.ID,
                Direction = ParameterDirection.Input
            });

            var rowsAffected = await command.ExecuteNonQueryAsync();

            return efResult + rowsAffected > 0;
        }

        //get

        public async Task<AccountOpening> GetAccountSelection(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    ACCOUNT_TYPE = x.ACCOUNT_TYPE
                    // You can add more fields if needed
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<AccountOpening> GetBasicInformationStep1(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    TITLE_FULL_NAME = x.TITLE_FULL_NAME,
                    FATHERORHUSBANDNAME = x.FATHERORHUSBANDNAME,
                    MOTHER_NAME = x.MOTHER_NAME,
                    PRINCIPLE_CNIC = x.PRINCIPLE_CNIC,
                    CNIC_ISSUANCE_DATE = x.CNIC_ISSUANCE_DATE,
                    PRINCIPLE_CNIC_EXP_DATE = x.PRINCIPLE_CNIC_EXP_DATE,
                    PRINCIPLE_CNIC_EXP_LIFE_TIME = x.PRINCIPLE_CNIC_EXP_LIFE_TIME,
                    // You can add more fields if needed
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<AccountOpening> GetBasicInformationStep2(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    DATE_OF_BIRTH = x.DATE_OF_BIRTH,
                    PLACE_OF_BIRTH = x.PLACE_OF_BIRTH,
                    GENDER = x.GENDER,
                    RESIDENTIAL_STATUS = x.RESIDENTIAL_STATUS,
                    ZAKAT_STATUS = x.ZAKAT_STATUS,
                    UPLOADCZ_50 = x.UPLOADCZ_50,
                    REFERRALCODE = x.REFERRALCODE,
                    // You can add more fields if needed
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<AccountOpening> GetContactDetail(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    PERMANENT_ADDRESS = x.PERMANENT_ADDRESS,
                    PERMANENT_CITY = x.PERMANENT_CITY,
                    PERMANENT_COUNTRY = x.PERMANENT_COUNTRY,
                    MOB_NO_COUNTRY_CODE = x.MOB_NO_COUNTRY_CODE,
                    MOBILE_NUMBER = x.MOBILE_NUMBER,
                    EMAIL = x.EMAIL,
                    IS_MAILING_SAME_AS_PERMANENT_ADDRESS = x.IS_MAILING_SAME_AS_PERMANENT_ADDRESS,
                    MOBILE_NUMBER_OWNERSHIP = x.MOBILE_NUMBER_OWNERSHIP,
                    MAILING_ADDRESS_1 = x.MAILING_ADDRESS_1,
                    MAILING_COUNTRY = x.MAILING_COUNTRY,
                    MAILING_CITY = x.MAILING_CITY
                    // You can add more fields if needed
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<AccountOpening> GetNextToKinInformation(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    NEXTOFKIN_NAME = x.NEXTOFKIN_NAME,
                    NEXTOFKIN_NO_COUNTRY_CODE = x.NEXTOFKIN_NO_COUNTRY_CODE,
                    NEXTOFKIN_MOB_NUMBER = x.NEXTOFKIN_MOB_NUMBER
                    // You can add more fields if needed
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<AccountOpening> GetBankDetails(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    BANK_NAME = x.BANK_NAME,
                    TYPE_OF_ACCOUNT = x.TYPE_OF_ACCOUNT,
                    WALLET_NAME = x.WALLET_NAME,
                    WALLET_NO = x.WALLET_NO,
                    IBAN_NO = x.IBAN_NO,
                    DIVIDEND_PAYOUT = x.DIVIDEND_PAYOUT
                    // You can add more fields if needed
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<AccountOpening> GetRiskProfileSavedValues(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    AGE = x.AGE,
                    MARITAL_STATUS = x.MARITAL_STATUS,
                    NO_OF_DEPENDENTS = x.NO_OF_DEPENDENTS,
                    RISKPROFILE_OCCUPATION = x.RISKPROFILE_OCCUPATION,
                    EDUCATION = x.EDUCATION,
                    RISK_APPETITE = x.RISK_APPETITE,
                    INVESTMENT_OBJECTIVE = x.INVESTMENT_OBJECTIVE,
                    INVESTMENT_HORIZON = x.INVESTMENT_HORIZON,
                    INVESTMENT_KNOWLEDGE = x.INVESTMENT_KNOWLEDGE,
                    FINANCIALPOSITION = x.FINANCIALPOSITION,
                    RISK_PROFILE_DECLARATION = x.RISK_PROFILE_DECLARATION
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<AccountOpening> GetRegulatoryKYC1(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    OCCUPATION = x.OCCUPATION,
                    PROFESSION = x.PROFESSION,
                    SOURCE_OF_INCOME = x.SOURCE_OF_INCOME,
                    NAME_OF_EMPLOYER_OR_BUSINESS = x.NAME_OF_EMPLOYER_OR_BUSINESS,
                    GROSS_ANNUAL_INCOME = x.GROSS_ANNUAL_INCOME,
                    EXPECTED_MONTHLY_INVESTEMENTAMOUNT = x.EXPECTED_MONTHLY_INVESTEMENTAMOUNT,
                    EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION = x.EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION,
                    EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION = x.EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<AccountOpening> GetRegulatoryKYC2(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    FINANCIAL_INSTITUUTION_REFUSAL = x.FINANCIAL_INSTITUUTION_REFUSAL,
                    ULTIMATE_BENEFICIARY = x.ULTIMATE_BENEFICIARY,
                    HOLDING_SENIOR_POSITION = x.HOLDING_SENIOR_POSITION,
                    INTERNATIONAL_RELATION_OR_PEP = x.INTERNATIONAL_RELATION_OR_PEP,
                    DEAL_WITH_HIGH_VALUE_ITEMS = x.DEAL_WITH_HIGH_VALUE_ITEMS,
                    FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS = x.FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS,
                    TRUE_INFORMATION_DECLARATION = x.TRUE_INFORMATION_DECLARATION
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<AccountOpening> GetFATCA(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    COUNTRY_OF_RESIDENT = x.COUNTRY_OF_RESIDENT,
                    IS_US_CITIZEN_RESIDENT_HAVEGREENCARD = x.IS_US_CITIZEN_RESIDENT_HAVEGREENCARD,
                    US_BORN = x.US_BORN,
                    INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA = x.INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA,
                    HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS = x.HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS,
                    HAVE_US_TELEPHONE_NUMBER = x.HAVE_US_TELEPHONE_NUMBER,
                    US_TAXPAYER_IDENTIFICATION_NUMBER = x.US_TAXPAYER_IDENTIFICATION_NUMBER,
                    W9_FORM_UPLOAD = x.W9_FORM_UPLOAD,
                    FATCA_DECLARATION = x.FATCA_DECLARATION,
                    NON_US_PERSON_DECLARATION = x.NON_US_PERSON_DECLARATION
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<AccountOpening> GetCRS(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    TAX_RESIDENT_COUNTRY = x.TAX_RESIDENT_COUNTRY,
                    TIN_NO = x.TIN_NO,
                    REASON_FOR_NO_TIN_NUMBER = x.REASON_FOR_NO_TIN_NUMBER,
                    HAVE_TIN = x.HAVE_TIN,
                    CRS_DECLARATION = x.CRS_DECLARATION
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<int> CalculateRiskProfileScore(long accountOpeningId)
        {
            var riskScoreData = await _context.AccountOpening
        .Where(a => a.ID == accountOpeningId)
        .Select(a => new
        {
            a.ID,
            Age = a.Age.SCORE,
            EducationScore = a.Education.SCORE,
            FinancialPosition = a.FinancialPosition.SCORE,
            InvestmentHorizon = a.InvestmentHorizon.SCORE,
            InvestmentKnowledge = a.InvestmentKnowledge.SCORE,
            InvestmentObjective = a.InvestmentObjective.SCORE,
            MartialStatus = a.MartialStatus.SCORE,
            NoOfDependents = a.NoOfDependents.SCORE,
            OccupationScore = a.RiskProfileOccupation.SCORE,
            RiskAppetite = a.RiskAppetite.SCORE
        })
        .FirstOrDefaultAsync();
            int totalScore =
        riskScoreData.Age +
        riskScoreData.EducationScore +
        riskScoreData.FinancialPosition +
        riskScoreData.InvestmentHorizon +
        riskScoreData.InvestmentKnowledge +
        riskScoreData.InvestmentObjective +
        riskScoreData.MartialStatus +
        riskScoreData.NoOfDependents +
        riskScoreData.OccupationScore +
        riskScoreData.RiskAppetite;
            return totalScore;
        }

        public async Task<AccountOpening> GetUploadedDocuments(long userId)
        {
            var result = await _accountOpeningSet
                .Where(x => x.ACCOUNTID == userId)
                .Select(x => new AccountOpening
                {
                    CNIC_UPLOAD_FRONT = x.CNIC_UPLOAD_FRONT,
                    CNIC_UPLOAD_BACK = x.CNIC_UPLOAD_BACK,
                    LIVEPHOTE_OR_SELFIE = x.LIVEPHOTE_OR_SELFIE,
                    PROOF_OF_SOURCE_OF_INCOME = x.PROOF_OF_SOURCE_OF_INCOME,
                    PAST_ONE_YEAR_BANKSTATEMENT = x.PAST_ONE_YEAR_BANKSTATEMENT,
                    FAMILY_MEMBER = x.FAMILY_MEMBER,
                    COMPANY_REGISTER = x.COMPANY_REGISTER,
                    INTERNATIONAL_NUMBER = x.INTERNATIONAL_NUMBER

                })
                .FirstOrDefaultAsync();
            return result;
        }
        //public async Task<AccountOpening> GetUploadedDigitalDocuments(long userId)
        //{
        //    var result = await _accountOpeningSet
        //        .Where(x => x.ACCOUNTID == userId)
        //        .Select(x => new AccountOpening
        //        {
        //            CNIC_UPLOAD_FRONT = x.CNIC_UPLOAD_FRONT,
        //            CNIC_UPLOAD_BACK = x.CNIC_UPLOAD_BACK,
        //            LIVEPHOTE_OR_SELFIE = x.LIVEPHOTE_OR_SELFIE

        //        })
        //        .FirstOrDefaultAsync();
        //    return result;
        //}
        public async Task<int> BasicInfoCompletedSteps(long userId)
        {
            var record = await _accountOpeningSet.FirstOrDefaultAsync(e => e.ACCOUNTID == userId);
            if (record == null)
                return 0;

            int BasicInfo = 0;
            if (record.BASICINFO_STEP1 == 1) BasicInfo++;
            if (record.BASICINFO_STEP2 == 1) BasicInfo++;

            return BasicInfo;
        }

        public async Task<int> PersonalDetailsCompletedSteps(long userId)
        {
            var record = await _accountOpeningSet.FirstOrDefaultAsync(e => e.ACCOUNTID == userId);
            if (record == null)
                return 0;

            int completedStepsCount = 0;
            if (record.PERSONALDETAILS_STEP1 == 1) completedStepsCount++;
            if (record.PERSONALDETAILS_STEP2 == 1) completedStepsCount++;
            if (record.PERSONALDETAILS_STEP3 == 1) completedStepsCount++;

            return completedStepsCount;
        }
        public async Task<int> RegulatoryKYCCompletedSteps(long userId)
        {
            var record = await _accountOpeningSet.FirstOrDefaultAsync(e => e.ACCOUNTID == userId);
            if (record == null)
                return 0;

            int completedStepsCount = 0;
            if (record.REGULATORYKYC_STEP1 == 1) completedStepsCount++;
            if (record.REGULATORYKYC_STEP2 == 1) completedStepsCount++;
            if (record.REGULATORYKYC_STEP3 == 1) completedStepsCount++;
            if (record.REGULATORYKYC_STEP4 == 1) completedStepsCount++;

            return completedStepsCount;
        }

        public async Task<List<(int Id, string Title, int TotalSteps, int CompletedSteps, int IsEnabled)>> StepsCounts(long userId)
        {
            int IsBasicInfoEnabled = 0;
            int IsPersonalDetailsEnabled = 0;
            int IsRiskProfileEnabled = 0;
            int IsRegulatoryKYCEnabled = 0;
            int IsUploadDocumentEnabled = 0;

            var record = await _accountOpeningSet
                .Where(e => e.ACCOUNTID == userId)
                .Select(e => new { e.ACCOUNT_TYPE, e.BASICINFO_STEP1, e.BASICINFO_STEP2, e.PERSONALDETAILS_STEP1, e.PERSONALDETAILS_STEP2, e.PERSONALDETAILS_STEP3, e.RISKPROFILE_STEP, e.REGULATORYKYC_STEP1, e.REGULATORYKYC_STEP2, e.REGULATORYKYC_STEP3, e.REGULATORYKYC_STEP4, e.UPLOADDOCUMENT_STEP })
                .FirstOrDefaultAsync();
            if (record == null)
                return new List<(int, string, int, int, int)>();

            int basicInfoCompletedSteps = 0;
            if (record.BASICINFO_STEP1 == 1) basicInfoCompletedSteps++;
            if (record.BASICINFO_STEP2 == 1) basicInfoCompletedSteps++;

            int personalDetailsCompletedSteps = 0;
            if (record.PERSONALDETAILS_STEP1 == 1) personalDetailsCompletedSteps++;
            if (record.PERSONALDETAILS_STEP2 == 1) personalDetailsCompletedSteps++;
            if (record.PERSONALDETAILS_STEP3 == 1) personalDetailsCompletedSteps++;

            int riskProfileCompletedSteps = 0;
            if (record.RISKPROFILE_STEP == 1) riskProfileCompletedSteps++;

            int regulatoryKYCCompletedSteps = 0;
            if (record.REGULATORYKYC_STEP1 == 1) regulatoryKYCCompletedSteps++;
            if (record.REGULATORYKYC_STEP2 == 1) regulatoryKYCCompletedSteps++;
            if (record.REGULATORYKYC_STEP3 == 1) regulatoryKYCCompletedSteps++;
            if (record.REGULATORYKYC_STEP4 == 1) regulatoryKYCCompletedSteps++;

            int uploadDocumentCompletedSteps = 0;
            if (record.UPLOADDOCUMENT_STEP == 1) uploadDocumentCompletedSteps++;

            if (record.ACCOUNT_TYPE != null)
                IsBasicInfoEnabled = 1;
            if (basicInfoCompletedSteps == _settings.BasicInfoSteps)
                IsPersonalDetailsEnabled = 1;
            if (personalDetailsCompletedSteps == _settings.PersonalDetailSteps)
                IsRiskProfileEnabled = 1;
            if (riskProfileCompletedSteps == _settings.RiskProfileSteps)
                IsRegulatoryKYCEnabled = 1;
            if (regulatoryKYCCompletedSteps == _settings.RegulatoryKycSteps)
                IsUploadDocumentEnabled = 1;

            return new List<(int, string, int, int, int)>
    {
        (1, "Basic Info",_settings.BasicInfoSteps, basicInfoCompletedSteps,IsBasicInfoEnabled),
        (2, "Personal Details", _settings.PersonalDetailSteps, personalDetailsCompletedSteps,IsPersonalDetailsEnabled),
        (3, "Risk Profile", _settings.RiskProfileSteps, riskProfileCompletedSteps,IsRiskProfileEnabled),
        (4, "Regulatory KYC", _settings.RegulatoryKycSteps, regulatoryKYCCompletedSteps,IsRegulatoryKYCEnabled),
        (5, "Upload Document", _settings.UploadDocumentSteps, uploadDocumentCompletedSteps,IsUploadDocumentEnabled),

    };
        }

        public async Task<bool> IsBasicInfoEnabled(long userId)
        {
            long recordId = await GetAccountOpeningIdByUserId(userId);
            var isEnabled = await _accountOpeningSet
                .Where(e => e.ID == recordId && e.ACCOUNT_TYPE != null)
                .CountAsync() > 0;
            return isEnabled;
        }
        public async Task<bool> IsPersonalDetailsEnabled(long userId)
        {
            long recordId = await GetAccountOpeningIdByUserId(userId);
            var isEnabled = await _accountOpeningSet
                .Where(e => e.ID == recordId && e.ACCOUNT_TYPE != null && e.BASICINFO_STEP1 == 1 && e.BASICINFO_STEP2 == 1)
                .CountAsync() > 0;
            return isEnabled;
        }
        public async Task<bool> IsRiskProfileEnabled(long userId)
        {
            long recordId = await GetAccountOpeningIdByUserId(userId);
            var isEnabled = await _accountOpeningSet
                .Where(e => e.ID == recordId && e.ACCOUNT_TYPE != null && e.PERSONALDETAILS_STEP1 == 1 && e.PERSONALDETAILS_STEP2 == 1 && e.PERSONALDETAILS_STEP3 == 1)
                .CountAsync() > 0;
            return isEnabled;
        }

        public async Task<bool> IsRegulatoryKYCEnabled(long userId)
        {
            long recordId = await GetAccountOpeningIdByUserId(userId);
            var isEnabled = await _accountOpeningSet
                .Where(e => e.ID == recordId && e.ACCOUNT_TYPE != null && e.RISKPROFILE_STEP == 1)
                .CountAsync() > 0;
            return isEnabled;
        }
        public async Task<bool> IsUploadDocumentEnabled(long userId)
        {
            long recordId = await GetAccountOpeningIdByUserId(userId);
            var isEnabled = await _accountOpeningSet
                .Where(e => e.ID == recordId && e.ACCOUNT_TYPE != null && e.REGULATORYKYC_STEP1 == 1 && e.REGULATORYKYC_STEP2 == 1 && e.REGULATORYKYC_STEP3 == 1 && e.REGULATORYKYC_STEP4 == 1)
                .CountAsync() > 0;
            return isEnabled;
        }
        //others
        public async Task<bool> DoesAnEntryExistsAgainstUserId(long userId)
        {
            return await _accountOpeningSet.CountAsync(e => e.ACCOUNTID == userId) > 0;
        }

        public async Task<long> GetAccountOpeningIdByUserId(long userId)
        {
            return await _accountOpeningSet.Where(e => e.ACCOUNTID == userId).Select(e => e.ID).FirstOrDefaultAsync();
        }

        public async Task<int> OnBoardingStatus (long userId)
        {
            var status = await _accountOpeningSet
          .Where(e => e.ACCOUNTID == userId)
          .Select(e => e.STATUS)
          .FirstOrDefaultAsync();

            return status;
        }
        public async Task<AccountOpening?> GetAccountOpening(long userId)
        {
            var dto = await _accountOpeningSet
        .Where(x => x.ACCOUNTID == userId)
        .Select(x => new AccountOpeningDTO
        {
            ID = x.ID,
            ACCOUNTID = x.ACCOUNTID,
            ACCOUNT_TYPE = x.ACCOUNT_TYPE,
            TITLE_FULL_NAME = x.TITLE_FULL_NAME,
            FATHERORHUSBANDNAME = x.FATHERORHUSBANDNAME,
            MOTHER_NAME = x.MOTHER_NAME,
            PRINCIPLE_CNIC = x.PRINCIPLE_CNIC,
            CNIC_ISSUANCE_DATE = x.CNIC_ISSUANCE_DATE,
            PRINCIPLE_CNIC_EXP_DATE = x.PRINCIPLE_CNIC_EXP_DATE,
            PRINCIPLE_CNIC_EXP_LIFE_TIME = x.PRINCIPLE_CNIC_EXP_LIFE_TIME,
            DATE_OF_BIRTH = x.DATE_OF_BIRTH,
            PLACE_OF_BIRTH = x.PLACE_OF_BIRTH,
            GENDER = x.GENDER,
            RESIDENTIAL_STATUS = x.RESIDENTIAL_STATUS,
            ZAKAT_STATUS = x.ZAKAT_STATUS,
            UPLOADCZ_50 = x.UPLOADCZ_50,
            REFERRALCODE = x.REFERRALCODE,
            PERMANENT_ADDRESS = x.PERMANENT_ADDRESS,
            PERMANENT_CITY = x.PERMANENT_CITY,
            PERMANENT_COUNTRY = x.PERMANENT_COUNTRY,
            MOB_NO_COUNTRY_CODE = x.MOB_NO_COUNTRY_CODE,
            MOBILE_NUMBER = x.MOBILE_NUMBER,
            EMAIL = x.EMAIL,
            IS_MAILING_SAME_AS_PERMANENT_ADDRESS = x.IS_MAILING_SAME_AS_PERMANENT_ADDRESS,
            MOBILE_NUMBER_OWNERSHIP = x.MOBILE_NUMBER_OWNERSHIP,
            MAILING_ADDRESS_1 = x.MAILING_ADDRESS_1,
            MAILING_COUNTRY = x.MAILING_COUNTRY,
            MAILING_CITY = x.MAILING_CITY,
            NEXTOFKIN_NAME = x.NEXTOFKIN_NAME,
            NEXTOFKIN_NO_COUNTRY_CODE = x.NEXTOFKIN_NO_COUNTRY_CODE,
            NEXTOFKIN_MOB_NUMBER = x.NEXTOFKIN_MOB_NUMBER,
            BANK_NAME = x.BANK_NAME,
            TYPE_OF_ACCOUNT = x.TYPE_OF_ACCOUNT,
            WALLET_NAME = x.WALLET_NAME,
            WALLET_NO = x.WALLET_NO,
            IBAN_NO = x.IBAN_NO,
            DIVIDEND_PAYOUT = x.DIVIDEND_PAYOUT,
            AGE = x.AGE,
            MARITAL_STATUS = x.MARITAL_STATUS,
            NO_OF_DEPENDENTS = x.NO_OF_DEPENDENTS,
            RISKPROFILE_OCCUPATION = x.RISKPROFILE_OCCUPATION,
            EDUCATION = x.EDUCATION,
            RISK_APPETITE = x.RISK_APPETITE,
            INVESTMENT_OBJECTIVE = x.INVESTMENT_OBJECTIVE,
            INVESTMENT_HORIZON = x.INVESTMENT_HORIZON,
            INVESTMENT_KNOWLEDGE = x.INVESTMENT_KNOWLEDGE,
            FINANCIALPOSITION = x.FINANCIALPOSITION,
            RISK_PROFILE_DECLARATION = x.RISK_PROFILE_DECLARATION,
            OCCUPATION = x.OCCUPATION,
            PROFESSION = x.PROFESSION,
            SOURCE_OF_INCOME = x.SOURCE_OF_INCOME,
            NAME_OF_EMPLOYER_OR_BUSINESS = x.NAME_OF_EMPLOYER_OR_BUSINESS,
            GROSS_ANNUAL_INCOME = x.GROSS_ANNUAL_INCOME,
            EXPECTED_MONTHLY_INVESTEMENTAMOUNT = x.EXPECTED_MONTHLY_INVESTEMENTAMOUNT,
            EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION = x.EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION,
            EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION = x.EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION,
            FINANCIAL_INSTITUUTION_REFUSAL = x.FINANCIAL_INSTITUUTION_REFUSAL,
            ULTIMATE_BENEFICIARY = x.ULTIMATE_BENEFICIARY,
            HOLDING_SENIOR_POSITION = x.HOLDING_SENIOR_POSITION,
            INTERNATIONAL_RELATION_OR_PEP = x.INTERNATIONAL_RELATION_OR_PEP,
            DEAL_WITH_HIGH_VALUE_ITEMS = x.DEAL_WITH_HIGH_VALUE_ITEMS,
            FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS = x.FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS,
            TRUE_INFORMATION_DECLARATION = x.TRUE_INFORMATION_DECLARATION,
            COUNTRY_OF_RESIDENT = x.COUNTRY_OF_RESIDENT,
            IS_US_CITIZEN_RESIDENT_HAVEGREENCARD = x.IS_US_CITIZEN_RESIDENT_HAVEGREENCARD,
            US_BORN = x.US_BORN,
            INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA = x.INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA,
            HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS = x.HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS,
            HAVE_US_TELEPHONE_NUMBER = x.HAVE_US_TELEPHONE_NUMBER,
            US_TAXPAYER_IDENTIFICATION_NUMBER = x.US_TAXPAYER_IDENTIFICATION_NUMBER,
            W9_FORM_UPLOAD = x.W9_FORM_UPLOAD,
            FATCA_DECLARATION = x.FATCA_DECLARATION,
            NON_US_PERSON_DECLARATION = x.NON_US_PERSON_DECLARATION,
            TAX_RESIDENT_COUNTRY = x.TAX_RESIDENT_COUNTRY,
            TIN_NO = x.TIN_NO,
            REASON_FOR_NO_TIN_NUMBER = x.REASON_FOR_NO_TIN_NUMBER,
            HAVE_TIN = x.HAVE_TIN,
            CRS_DECLARATION = x.CRS_DECLARATION,
            CNIC_UPLOAD_FRONT = x.CNIC_UPLOAD_FRONT,
            CNIC_UPLOAD_BACK = x.CNIC_UPLOAD_BACK,
            LIVEPHOTE_OR_SELFIE = x.LIVEPHOTE_OR_SELFIE,
            PROOF_OF_SOURCE_OF_INCOME = x.PROOF_OF_SOURCE_OF_INCOME,
            PAST_ONE_YEAR_BANKSTATEMENT = x.PAST_ONE_YEAR_BANKSTATEMENT,
            FAMILY_MEMBER = x.FAMILY_MEMBER,
            COMPANY_REGISTER = x.COMPANY_REGISTER,
            INTERNATIONAL_NUMBER = x.INTERNATIONAL_NUMBER

        })
             .FirstOrDefaultAsync();

            if (dto == null)
                return null;

            // Manually map DTO to entity (or use AutoMapper if available)
            return new AccountOpening
            {
              ID = dto.ID,
            ACCOUNTID = dto.ACCOUNTID,
            ACCOUNT_TYPE = dto.ACCOUNT_TYPE,
            TITLE_FULL_NAME = dto.TITLE_FULL_NAME,
            FATHERORHUSBANDNAME = dto.FATHERORHUSBANDNAME,
            MOTHER_NAME = dto.MOTHER_NAME,
            PRINCIPLE_CNIC = dto.PRINCIPLE_CNIC,
            CNIC_ISSUANCE_DATE = dto.CNIC_ISSUANCE_DATE,
            PRINCIPLE_CNIC_EXP_DATE = dto.PRINCIPLE_CNIC_EXP_DATE,
            PRINCIPLE_CNIC_EXP_LIFE_TIME = dto.PRINCIPLE_CNIC_EXP_LIFE_TIME,
            DATE_OF_BIRTH = dto.DATE_OF_BIRTH,
            PLACE_OF_BIRTH = dto.PLACE_OF_BIRTH,
            GENDER = dto.GENDER,
            RESIDENTIAL_STATUS = dto.RESIDENTIAL_STATUS,
            ZAKAT_STATUS = dto.ZAKAT_STATUS,
            UPLOADCZ_50 = dto.UPLOADCZ_50,
            REFERRALCODE = dto.REFERRALCODE,
            PERMANENT_ADDRESS = dto.PERMANENT_ADDRESS,
            PERMANENT_CITY = dto.PERMANENT_CITY,
            PERMANENT_COUNTRY = dto.PERMANENT_COUNTRY,
            MOB_NO_COUNTRY_CODE = dto.MOB_NO_COUNTRY_CODE,
            MOBILE_NUMBER = dto.MOBILE_NUMBER,
            EMAIL = dto.EMAIL,
            IS_MAILING_SAME_AS_PERMANENT_ADDRESS = dto.IS_MAILING_SAME_AS_PERMANENT_ADDRESS,
            MOBILE_NUMBER_OWNERSHIP = dto.MOBILE_NUMBER_OWNERSHIP,
            MAILING_ADDRESS_1 = dto.MAILING_ADDRESS_1,
            MAILING_COUNTRY = dto.MAILING_COUNTRY,
            MAILING_CITY = dto.MAILING_CITY,
            NEXTOFKIN_NAME = dto.NEXTOFKIN_NAME,
            NEXTOFKIN_NO_COUNTRY_CODE = dto.NEXTOFKIN_NO_COUNTRY_CODE,
            NEXTOFKIN_MOB_NUMBER = dto.NEXTOFKIN_MOB_NUMBER,
            BANK_NAME = dto.BANK_NAME,
            TYPE_OF_ACCOUNT = dto.TYPE_OF_ACCOUNT,
            WALLET_NAME = dto.WALLET_NAME,
            WALLET_NO = dto.WALLET_NO,
            IBAN_NO = dto.IBAN_NO,
            DIVIDEND_PAYOUT = dto.DIVIDEND_PAYOUT,
            AGE = dto.AGE,
            MARITAL_STATUS = dto.MARITAL_STATUS,
            NO_OF_DEPENDENTS = dto.NO_OF_DEPENDENTS,
            RISKPROFILE_OCCUPATION = dto.RISKPROFILE_OCCUPATION,
            EDUCATION = dto.EDUCATION,
            RISK_APPETITE = dto.RISK_APPETITE,
            INVESTMENT_OBJECTIVE = dto.INVESTMENT_OBJECTIVE,
            INVESTMENT_HORIZON = dto.INVESTMENT_HORIZON,
            INVESTMENT_KNOWLEDGE = dto.INVESTMENT_KNOWLEDGE,
            FINANCIALPOSITION = dto.FINANCIALPOSITION,
            RISK_PROFILE_DECLARATION = dto.RISK_PROFILE_DECLARATION,
            OCCUPATION = dto.OCCUPATION,
            PROFESSION = dto.PROFESSION,
            SOURCE_OF_INCOME = dto.SOURCE_OF_INCOME,
            NAME_OF_EMPLOYER_OR_BUSINESS = dto.NAME_OF_EMPLOYER_OR_BUSINESS,
            GROSS_ANNUAL_INCOME = dto.GROSS_ANNUAL_INCOME,
            EXPECTED_MONTHLY_INVESTEMENTAMOUNT = dto.EXPECTED_MONTHLY_INVESTEMENTAMOUNT,
            EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION = dto.EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION,
            EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION = dto.EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION,
            FINANCIAL_INSTITUUTION_REFUSAL = dto.FINANCIAL_INSTITUUTION_REFUSAL,
            ULTIMATE_BENEFICIARY = dto.ULTIMATE_BENEFICIARY,
            HOLDING_SENIOR_POSITION = dto.HOLDING_SENIOR_POSITION,
            INTERNATIONAL_RELATION_OR_PEP = dto.INTERNATIONAL_RELATION_OR_PEP,
            DEAL_WITH_HIGH_VALUE_ITEMS = dto.DEAL_WITH_HIGH_VALUE_ITEMS,
            FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS = dto.FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS,
            TRUE_INFORMATION_DECLARATION = dto.TRUE_INFORMATION_DECLARATION,
            COUNTRY_OF_RESIDENT = dto.COUNTRY_OF_RESIDENT,
            IS_US_CITIZEN_RESIDENT_HAVEGREENCARD = dto.IS_US_CITIZEN_RESIDENT_HAVEGREENCARD,
            US_BORN = dto.US_BORN,
            INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA = dto.INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA,
            HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS = dto.HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS,
            HAVE_US_TELEPHONE_NUMBER = dto.HAVE_US_TELEPHONE_NUMBER,
            US_TAXPAYER_IDENTIFICATION_NUMBER = dto.US_TAXPAYER_IDENTIFICATION_NUMBER,
            W9_FORM_UPLOAD = dto.W9_FORM_UPLOAD,
            FATCA_DECLARATION = dto.FATCA_DECLARATION,
            NON_US_PERSON_DECLARATION = dto.NON_US_PERSON_DECLARATION,
            TAX_RESIDENT_COUNTRY = dto.TAX_RESIDENT_COUNTRY,
            TIN_NO = dto.TIN_NO,
            REASON_FOR_NO_TIN_NUMBER = dto.REASON_FOR_NO_TIN_NUMBER,
            HAVE_TIN = dto.HAVE_TIN,
            CRS_DECLARATION = dto.CRS_DECLARATION,
            CNIC_UPLOAD_FRONT = dto.CNIC_UPLOAD_FRONT,
            CNIC_UPLOAD_BACK = dto.CNIC_UPLOAD_BACK,
            LIVEPHOTE_OR_SELFIE = dto.LIVEPHOTE_OR_SELFIE,
            PROOF_OF_SOURCE_OF_INCOME = dto.PROOF_OF_SOURCE_OF_INCOME,
            PAST_ONE_YEAR_BANKSTATEMENT = dto.PAST_ONE_YEAR_BANKSTATEMENT,
            FAMILY_MEMBER = dto.FAMILY_MEMBER,
            COMPANY_REGISTER = dto.COMPANY_REGISTER,
            INTERNATIONAL_NUMBER = dto.INTERNATIONAL_NUMBER
            };
        }


        //private

        private void AttachAndMarkModified(AccountOpening model, params Expression<Func<AccountOpening, object>>[] properties)
        {
            var stub = new AccountOpening { ID = model.ID };
            _context.Attach(stub);

            foreach (var propExpr in properties)
            {
                var propertyName = ((MemberExpression)(propExpr.Body is UnaryExpression unary ? unary.Operand : propExpr.Body)).Member.Name;
                var value = typeof(AccountOpening).GetProperty(propertyName)?.GetValue(model);
                typeof(AccountOpening).GetProperty(propertyName)?.SetValue(stub, value);
                _context.Entry(stub).Property(propertyName).IsModified = true;
            }
        }

    }
}
