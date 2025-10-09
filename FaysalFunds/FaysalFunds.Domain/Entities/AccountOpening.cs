using FaysalFunds.Domain.Entities.RiskProfileDropdowns;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaysalFunds.Domain.Entities
{
    [Table("ACCOUNTOPENING")]
    public class AccountOpening
    {
        //Db
        [ForeignKey("ACCOUNTID")]
        public long ACCOUNTID { get; set; }
        // Nullable navigation property to prevent issues
        public Account? ACCOUNT
        {
            get; set;
        }
        //Basic Information
        public long ID { get; set; }
        public int? ACCOUNT_TYPE { get; set; } // dropdown
        public string? SALUTATION { get; set; }
        public string? TITLE_FULL_NAME { get; set; }
        public string? FATHERORHUSBANDNAME { get; set; }
        public string? MOTHER_NAME { get; set; }
        public string? PRINCIPLE_CNIC { get; set; }
        public DateTime? CNIC_ISSUANCE_DATE { get; set; }
        public DateTime? PRINCIPLE_CNIC_EXP_DATE { get; set; }
        public int PRINCIPLE_CNIC_EXP_LIFE_TIME { get; set; } //1 or 0
        public DateTime? DATE_OF_BIRTH { get; set; }
        public int? PLACE_OF_BIRTH { get; set; } // dropdown
        public string? GENDER { get; set; }
        public string? RESIDENTIAL_STATUS { get; set; }
        public string? ZAKAT_STATUS { get; set; }
        public byte[]? UPLOADCZ_50 { get; set; }
        public string? REFERRALCODE { get; set; }
        //Contact Details
        public string? PERMANENT_ADDRESS { get; set; } 
        public int? PERMANENT_CITY { get; set; } // dropdown
        public int? PERMANENT_COUNTRY { get; set; } // dropdown
        public int IS_MAILING_SAME_AS_PERMANENT_ADDRESS { get; set; } //1 or 0
        public string? MAILING_ADDRESS_1 { get; set; }
        public int? MAILING_CITY { get; set; } // dropdown
        public int? MAILING_COUNTRY { get; set; } // dropdown
        public string? MOB_NO_COUNTRY_CODE { get; set; }
        public string? MOB_OPERATOR_CODE { get; set; }
        public string? MOBILE_NUMBER { get; set; }
        public int? MOBILE_NUMBER_OWNERSHIP { get; set; } //dropdown
        public byte[]? MOBILE_OWNERSHIP_PROOF { get; set; } 
        public string? EMAIL { get; set; }
       

        //Bank Account Information
        public string? TYPE_OF_ACCOUNT { get; set; }
        public int? BANK_NAME { get; set; } //dropdown
        public int? BRANCH_CITY { get; set; } //dropdown    //Not mentioned
        public string? BRANCH_NAME { get; set; }    //Not mentioned
        public int? WALLET_NAME { get; set; } //dropdown
        public string? IBAN_NO { get; set; }
        public string? ACCOUNT_NUMBER { get; set; }     //Not mentioned
        public string? WALLET_NO { get; set; }
        public string? DIVIDEND_PAYOUT { get; set; } // Re-Investment or Encashment

        //Next of Kin Information
        public string? NEXTOFKIN_NAME { get; set; }
        public string? NEXTOFKIN_NO_COUNTRY_CODE { get; set; }
        public string? NEXTOFKIN_OPERATOR_CODE { get; set; }
        public string? NEXTOFKIN_MOB_NUMBER { get; set; }

        //Risk Profile
        [ForeignKey(nameof(Age))]
        public int? AGE { get; set; } //dropdown
        [ForeignKey(nameof(MartialStatus))]
        public int? MARITAL_STATUS { get; set; } //dropdown
        [ForeignKey(nameof(NoOfDependents))]
        public int? NO_OF_DEPENDENTS { get; set; } //dropdown
        [ForeignKey(nameof(RiskProfileOccupation))]
        public int? RISKPROFILE_OCCUPATION { get; set; }
        [ForeignKey(nameof(Education))]
        public int? EDUCATION { get; set; } //dropdown
        [ForeignKey(nameof(RiskAppetite))]
        public int? RISK_APPETITE { get; set; } //dropdown
        [ForeignKey(nameof(InvestmentObjective))]
        public int? INVESTMENT_OBJECTIVE { get; set; } //dropdown
        [ForeignKey(nameof(InvestmentHorizon))]
        public int? INVESTMENT_HORIZON { get; set; } //dropdown
        [ForeignKey(nameof(InvestmentKnowledge))]
        public int? INVESTMENT_KNOWLEDGE { get; set; } //dropdown
        [ForeignKey(nameof(FinancialPosition))]
        public int? FINANCIALPOSITION { get; set; } //dropdown
        public int RISK_PROFILE_DECLARATION { get; set; } //1 or 0

        //Regulatory KYC
        public int? OCCUPATION { get; set; }//dropdown
        public int? PROFESSION { get; set; }//dropdown
        public int? SOURCE_OF_INCOME { get; set; }//dropdown
        public string? NAME_OF_EMPLOYER_OR_BUSINESS { get; set; }
        public decimal? GROSS_ANNUAL_INCOME { get; set; }
        public int INTERNATIONAL_RELATION_OR_PEP { get; set; } //1 or 0
        public int? EXPECTED_MONTHLY_INVESTEMENTAMOUNT { get; set; }//dropdown
        public int? EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION { get; set; }//dropdown
        public int? EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION { get; set; }//dropdown
        public int FINANCIAL_INSTITUUTION_REFUSAL { get; set; } //1 or 0
        public int ULTIMATE_BENEFICIARY { get; set; } //1 or 0
        public int HOLDING_SENIOR_POSITION { get; set; } //1 or 0
        public int DEAL_WITH_HIGH_VALUE_ITEMS { get; set; }  //1 or 0
        public int FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS { get; set; } //1 or 0
        public int TRUE_INFORMATION_DECLARATION { get; set; } //1 or 0

        //Required Documents
        public byte[]? CNIC_UPLOAD_FRONT { get; set; }
        public byte[]? CNIC_UPLOAD_BACK { get; set; }
        public byte[]? LIVEPHOTE_OR_SELFIE { get; set; }

        public byte[]? ZAKAT_UPLOAD { get; set; }
        public byte[]? PROOF_OF_SOURCE_OF_INCOME { get; set; }
        public byte[]? PAST_ONE_YEAR_BANKSTATEMENT { get; set; }
        public byte[]? OTHER_DOC_UPLOAD_1 { get; set; }
        public byte[]? OTHER_DOC_UPLOAD_2 { get; set; }
        public byte[]? FAMILY_MEMBER { get; set; }
        public byte[]? COMPANY_REGISTER { get; set; }
        public byte[]? INTERNATIONAL_NUMBER { get; set; }


        //FATCA
        public string? COUNTRY_OF_RESIDENT { get; set; }
        public int IS_US_CITIZEN_RESIDENT_HAVEGREENCARD { get; set; } //1 or 0
        public string? US_TAXPAYER_IDENTIFICATION_NUMBER { get; set; }
        public byte[]? W9_FORM_UPLOAD { get; set; }
        public int US_BORN { get; set; }//1 or 0
        public int? NON_US_PERSON_0DECLARATION { get; set; }//1 or 0
        public int INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA { get; set; }//1 or 0    
        public int HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS { get; set; }//1 or 0     
        public int HAVE_US_TELEPHONE_NUMBER { get; set; }//1 or 0  
        public int FATCA_DECLARATION { get; set; } //1 or 0
        public int NON_US_PERSON_DECLARATION { get; set; } //1 or 0


        //CRS Form
        public int? TAX_RESIDENT_COUNTRY { get; set; } //dropdown
        public string? TIN_NO { get; set; }
        public int? REASON_FOR_NO_TIN_NUMBER { get; set; }//dropdown
        public int HAVE_TIN { get; set; } //1 or 0
        public int CRS_DECLARATION { get; set; } //1 or 0

        //Steps Completion Statuses
        public int BASICINFO_STEP1 { get; set; }//1 or 0
        public int BASICINFO_STEP2 { get; set; }//1 or 0
        public int PERSONALDETAILS_STEP1 { get; set; }//1 or 0
        public int PERSONALDETAILS_STEP2 { get; set; }//1 or 0
        public int PERSONALDETAILS_STEP3 { get; set; }//1 or 0
        public int RISKPROFILE_STEP { get; set; }//1 or 0
        public int REGULATORYKYC_STEP1 { get; set; }//1 or 0
        public int REGULATORYKYC_STEP2 { get; set; }//1 or 0
        public int REGULATORYKYC_STEP3 { get; set; }//1 or 0
        public int REGULATORYKYC_STEP4 { get; set; }//1 or 0
        public int UPLOADDOCUMENT_STEP { get; set; }//1 or 0


        //RiskProfile Navigational Properties
        public Age Age { get; set; }
        public Education Education { get; set; }
        public FinancialPosition FinancialPosition { get; set; }
        public InvestmentHorizon InvestmentHorizon { get; set; }
        public InvestmentKnowledge InvestmentKnowledge { get; set; }
        public InvestmentObjective InvestmentObjective { get; set; }
        public MartialStatus MartialStatus { get; set; }
        public NoOfDependents NoOfDependents { get; set; }
        public RiskProfileOccupation RiskProfileOccupation { get; set; }
        public RiskAppetite RiskAppetite { get; set; }

        //Awaiting for approval, approved, rejected
        public int STATUS { get; set; }
    }
}