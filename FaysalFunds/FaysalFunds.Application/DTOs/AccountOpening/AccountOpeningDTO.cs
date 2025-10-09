using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.DTOs.AccountOpening
{
    public class AccountOpeningDTO  //use for Excel file
    {
        public long ID { get; set; }
        public long ACCOUNTID { get; set; }
        public int? ACCOUNT_TYPE { get; set; }
        public string TITLE_FULL_NAME { get; set; }
        public string FATHERORHUSBANDNAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string PRINCIPLE_CNIC { get; set; }
        public DateTime? CNIC_ISSUANCE_DATE { get; set; }
        public DateTime? PRINCIPLE_CNIC_EXP_DATE { get; set; }
        public int PRINCIPLE_CNIC_EXP_LIFE_TIME { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public int? PLACE_OF_BIRTH { get; set; }
        public string GENDER { get; set; }
        public string RESIDENTIAL_STATUS { get; set; }
        public string ZAKAT_STATUS { get; set; }
        public byte[]? UPLOADCZ_50 { get; set; }
        public string REFERRALCODE { get; set; }
        public string PERMANENT_ADDRESS { get; set; }
        public int? PERMANENT_CITY { get; set; }
        public int? PERMANENT_COUNTRY { get; set; }
        public string MOB_NO_COUNTRY_CODE { get; set; }
        public string MOBILE_NUMBER { get; set; }
        public string EMAIL { get; set; }
        public int IS_MAILING_SAME_AS_PERMANENT_ADDRESS { get; set; }
        public int? MOBILE_NUMBER_OWNERSHIP { get; set; }
        public string MAILING_ADDRESS_1 { get; set; }
        public int? MAILING_COUNTRY { get; set; }
        public int? MAILING_CITY { get; set; }
        public string NEXTOFKIN_NAME { get; set; }
        public string NEXTOFKIN_NO_COUNTRY_CODE { get; set; }
        public string NEXTOFKIN_MOB_NUMBER { get; set; }
        public int? BANK_NAME { get; set; }
        public string TYPE_OF_ACCOUNT { get; set; }
        public int? WALLET_NAME { get; set; }
        public string WALLET_NO { get; set; }
        public string IBAN_NO { get; set; }
        public string DIVIDEND_PAYOUT { get; set; }
        public int? AGE { get; set; }
        public int? MARITAL_STATUS { get; set; }
        public int? NO_OF_DEPENDENTS { get; set; }
        public int? RISKPROFILE_OCCUPATION { get; set; }
        public int? EDUCATION { get; set; }
        public int? RISK_APPETITE { get; set; }
        public int? INVESTMENT_OBJECTIVE { get; set; }
        public int? INVESTMENT_HORIZON { get; set; }
        public int? INVESTMENT_KNOWLEDGE { get; set; }
        public int? FINANCIALPOSITION { get; set; }
        public int RISK_PROFILE_DECLARATION { get; set; }
        public int? OCCUPATION { get; set; }
        public int? PROFESSION { get; set; }
        public int? SOURCE_OF_INCOME { get; set; }
        public string? NAME_OF_EMPLOYER_OR_BUSINESS { get; set; }
        public decimal? GROSS_ANNUAL_INCOME { get; set; }
        public int? EXPECTED_MONTHLY_INVESTEMENTAMOUNT { get; set; }
        public int? EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION { get; set; }
        public int? EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION { get; set; }
        public int FINANCIAL_INSTITUUTION_REFUSAL { get; set; }
        public int ULTIMATE_BENEFICIARY { get; set; }
        public int HOLDING_SENIOR_POSITION { get; set; }
        public int INTERNATIONAL_RELATION_OR_PEP { get; set; }
        public int DEAL_WITH_HIGH_VALUE_ITEMS { get; set; }
        public int FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS { get; set; }
        public int TRUE_INFORMATION_DECLARATION { get; set; }
        public string COUNTRY_OF_RESIDENT { get; set; }
        public int IS_US_CITIZEN_RESIDENT_HAVEGREENCARD { get; set; }
        public int US_BORN { get; set; }
        public int INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA { get; set; }
        public int HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS { get; set; }
        public int HAVE_US_TELEPHONE_NUMBER { get; set; }
        public string US_TAXPAYER_IDENTIFICATION_NUMBER { get; set; }
        public byte[]? W9_FORM_UPLOAD { get; set; }
        public int FATCA_DECLARATION { get; set; }
        public int NON_US_PERSON_DECLARATION { get; set; }
        public int? TAX_RESIDENT_COUNTRY { get; set; }
        public string TIN_NO { get; set; }
        public int? REASON_FOR_NO_TIN_NUMBER { get; set; }
        public int HAVE_TIN { get; set; }
        public int CRS_DECLARATION { get; set; }
        public byte[]? CNIC_UPLOAD_FRONT { get; set; }
        public byte[]? CNIC_UPLOAD_BACK { get; set; }
        public byte[]? LIVEPHOTE_OR_SELFIE { get; set; }
        public byte[]? PROOF_OF_SOURCE_OF_INCOME { get; set; }
        public byte[]? PAST_ONE_YEAR_BANKSTATEMENT { get; set; }
        public byte[]? FAMILY_MEMBER { get; set; }
        public byte[]? COMPANY_REGISTER { get; set; }
        public byte[]? INTERNATIONAL_NUMBER { get; set; }
    }
}
