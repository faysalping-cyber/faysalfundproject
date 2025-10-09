using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.AccountOpening;
using FaysalFunds.Application.Services.IServices;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using OfficeOpenXml;
using Org.BouncyCastle.Utilities;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Authentication;

namespace FaysalFunds.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly DropdownService _dropdownService;
        private readonly RiskProfileService _riskprofileServices;
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, DropdownService dropdownService, RiskProfileService riskprofileServices)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger ?? throw new ApiException(nameof(logger));
            _dropdownService = dropdownService;
            _riskprofileServices = riskprofileServices;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            using var smtp = new System.Net.Mail.SmtpClient("smtp.office365.com", 587)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true,  
                Timeout = 1000000
            };

            using var mail = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = "One-Time Password (OTP) – Faysal Funds",
                Body = $@"Dear Valued Customer,

        Your OTP is {otp} and is only valid for 5 minutes.

        Do not share this with anyone. If you have not requested an OTP, kindly call Faysal Funds Islami Customer Care at 021-111 329 725.

        Regards,
        Faysal Funds",
                IsBodyHtml = false
            };

            mail.To.Add(toEmail);

            await smtp.SendMailAsync(mail);
        }

        //basic email sending

        //   public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false, bool includeExcelAttachment = false, AccountOpening onboardingData = null)    {
        //    if (string.IsNullOrWhiteSpace(toEmail))
        //        throw new ApiException("Recipient email cannot be empty.");
        //    if (string.IsNullOrWhiteSpace(subject))
        //        throw new ApiException("Subject cannot be empty.");
        //    if (string.IsNullOrWhiteSpace(body))
        //        throw new ApiException("Body cannot be empty.");

        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        //    message.To.Add(MailboxAddress.Parse(toEmail));
        //    message.Subject = subject;

        //    var builder = new BodyBuilder
        //    {
        //        HtmlBody = isHtml ? body : null,
        //        TextBody = !isHtml ? body : null
        //    };

        //    message.Body = builder.ToMessageBody();

        //    using var smtp = new MailKit.Net.Smtp.SmtpClient();

        //        try
        //        {

        //            _logger.LogInformation("Connecting to SMTP server...");
        //            await smtp.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
        //            _logger.LogInformation("Connected to SMTP server.");

        //            _logger.LogInformation("Authenticating with SMTP...");
        //            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
        //            _logger.LogInformation("SMTP authentication successful.");

        //            _logger.LogInformation("Sending email to {0} ...", toEmail);
        //            await smtp.SendAsync(message);
        //            _logger.LogInformation("Email successfully handed over to Office365 SMTP server.");

        //            await smtp.DisconnectAsync(true);
        //            _logger.LogInformation("Disconnected from SMTP server.");
        //        }
        //        catch (Exception ex)
        //    {
        //            _logger.LogError(ex, "❌ Email send failed. ExceptionType={0}, Message={1}", ex.GetType().FullName, ex.Message);
        //            if (ex.InnerException != null)
        //            {
        //                _logger.LogError("➡️ Inner Exception: {0}", ex.InnerException.Message);
        //            }
        //            throw;
        //    }
        //}



        public async Task SendEmailAsync(string toEmail, AccountOpening onboardingData)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ApiException("Recipient email cannot be empty.");
            if (onboardingData == null)
                throw new ApiException("Onboarding data cannot be null.");

            // Hardcoded subject & body
            var subject = "Onboarding Profile";
            var body = $"Dear Service Provider,<br/><br/>New onboarding profile details have been attached in the Excel file. Kindly review this file.<br/><br/>Regards,<br/>Customer Name</br>{onboardingData.TITLE_FULL_NAME}";


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            // ✅ Dropdowns service call
            var dropdownsResponse = await _dropdownService.GetDropdownsAsync();
            var dropdowns = dropdownsResponse?.Data ?? new Dictionary<string, List<DropDownDTO>>();
            var riskProfileResponse = await _riskprofileServices.GetRiskProfileDropdownsAsync();
            var riskProfileQuestions = riskProfileResponse?.Data?.Body ?? new List<QuestionnaireItemDTO>();

            // ✅ Entity → DTO mapping
            var dto = new AccountOpeningDTO
            {
                ID = onboardingData.ID,
                ACCOUNTID = onboardingData.ACCOUNTID,
                ACCOUNT_TYPE = onboardingData.ACCOUNT_TYPE,
                TITLE_FULL_NAME = onboardingData.TITLE_FULL_NAME,
                FATHERORHUSBANDNAME = onboardingData.FATHERORHUSBANDNAME,
                MOTHER_NAME = onboardingData.MOTHER_NAME,
                PRINCIPLE_CNIC = onboardingData.PRINCIPLE_CNIC,
                CNIC_ISSUANCE_DATE = onboardingData.CNIC_ISSUANCE_DATE,
                PRINCIPLE_CNIC_EXP_DATE = onboardingData.PRINCIPLE_CNIC_EXP_DATE,
                PRINCIPLE_CNIC_EXP_LIFE_TIME = onboardingData.PRINCIPLE_CNIC_EXP_LIFE_TIME,
                DATE_OF_BIRTH = onboardingData.DATE_OF_BIRTH,
                PLACE_OF_BIRTH = onboardingData.PLACE_OF_BIRTH,
                GENDER = onboardingData.GENDER,
                RESIDENTIAL_STATUS = onboardingData.RESIDENTIAL_STATUS,
                ZAKAT_STATUS = onboardingData.ZAKAT_STATUS,
                UPLOADCZ_50 = onboardingData.UPLOADCZ_50,
                REFERRALCODE = onboardingData.REFERRALCODE,
                PERMANENT_ADDRESS = onboardingData.PERMANENT_ADDRESS,
                PERMANENT_CITY = onboardingData.PERMANENT_CITY,
                PERMANENT_COUNTRY = onboardingData.PERMANENT_COUNTRY,
                MOB_NO_COUNTRY_CODE = onboardingData.MOB_NO_COUNTRY_CODE,
                MOBILE_NUMBER = onboardingData.MOBILE_NUMBER,
                EMAIL = onboardingData.EMAIL,
                IS_MAILING_SAME_AS_PERMANENT_ADDRESS = onboardingData.IS_MAILING_SAME_AS_PERMANENT_ADDRESS,
                MOBILE_NUMBER_OWNERSHIP = onboardingData.MOBILE_NUMBER_OWNERSHIP,
                MAILING_ADDRESS_1 = onboardingData.MAILING_ADDRESS_1,
                MAILING_COUNTRY = onboardingData.MAILING_COUNTRY,
                MAILING_CITY = onboardingData.MAILING_CITY,
                NEXTOFKIN_NAME = onboardingData.NEXTOFKIN_NAME,
                NEXTOFKIN_NO_COUNTRY_CODE = onboardingData.NEXTOFKIN_NO_COUNTRY_CODE,
                NEXTOFKIN_MOB_NUMBER = onboardingData.NEXTOFKIN_MOB_NUMBER,
                BANK_NAME = onboardingData.BANK_NAME,
                TYPE_OF_ACCOUNT = onboardingData.TYPE_OF_ACCOUNT,
                WALLET_NAME = onboardingData.WALLET_NAME,
                WALLET_NO = onboardingData.WALLET_NO,
                IBAN_NO = onboardingData.IBAN_NO,
                DIVIDEND_PAYOUT = onboardingData.DIVIDEND_PAYOUT,
                AGE = onboardingData.AGE,
                MARITAL_STATUS = onboardingData.MARITAL_STATUS,
                NO_OF_DEPENDENTS = onboardingData.NO_OF_DEPENDENTS,
                RISKPROFILE_OCCUPATION = onboardingData.RISKPROFILE_OCCUPATION,
                EDUCATION = onboardingData.EDUCATION,
                RISK_APPETITE = onboardingData.RISK_APPETITE,
                INVESTMENT_OBJECTIVE = onboardingData.INVESTMENT_OBJECTIVE,
                INVESTMENT_HORIZON = onboardingData.INVESTMENT_HORIZON,
                INVESTMENT_KNOWLEDGE = onboardingData.INVESTMENT_KNOWLEDGE,
                FINANCIALPOSITION = onboardingData.FINANCIALPOSITION,
                RISK_PROFILE_DECLARATION = onboardingData.RISK_PROFILE_DECLARATION,
                OCCUPATION = onboardingData.OCCUPATION,
                PROFESSION = onboardingData.PROFESSION,
                SOURCE_OF_INCOME = onboardingData.SOURCE_OF_INCOME,
                NAME_OF_EMPLOYER_OR_BUSINESS = onboardingData.NAME_OF_EMPLOYER_OR_BUSINESS,
                GROSS_ANNUAL_INCOME = onboardingData.GROSS_ANNUAL_INCOME,
                EXPECTED_MONTHLY_INVESTEMENTAMOUNT = onboardingData.EXPECTED_MONTHLY_INVESTEMENTAMOUNT,
                EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION = onboardingData.EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION,
                EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION = onboardingData.EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION,
                FINANCIAL_INSTITUUTION_REFUSAL = onboardingData.FINANCIAL_INSTITUUTION_REFUSAL,
                ULTIMATE_BENEFICIARY = onboardingData.ULTIMATE_BENEFICIARY,
                HOLDING_SENIOR_POSITION = onboardingData.HOLDING_SENIOR_POSITION,
                INTERNATIONAL_RELATION_OR_PEP = onboardingData.INTERNATIONAL_RELATION_OR_PEP,
                DEAL_WITH_HIGH_VALUE_ITEMS = onboardingData.DEAL_WITH_HIGH_VALUE_ITEMS,
                FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS = onboardingData.FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS,
                TRUE_INFORMATION_DECLARATION = onboardingData.TRUE_INFORMATION_DECLARATION,
                COUNTRY_OF_RESIDENT = onboardingData.COUNTRY_OF_RESIDENT,
                IS_US_CITIZEN_RESIDENT_HAVEGREENCARD = onboardingData.IS_US_CITIZEN_RESIDENT_HAVEGREENCARD,
                US_BORN = onboardingData.US_BORN,
                INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA = onboardingData.INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA,
                HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS = onboardingData.HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS,
                HAVE_US_TELEPHONE_NUMBER = onboardingData.HAVE_US_TELEPHONE_NUMBER,
                US_TAXPAYER_IDENTIFICATION_NUMBER = onboardingData.US_TAXPAYER_IDENTIFICATION_NUMBER,
                W9_FORM_UPLOAD = onboardingData.W9_FORM_UPLOAD,
                FATCA_DECLARATION = onboardingData.FATCA_DECLARATION,
                NON_US_PERSON_DECLARATION = onboardingData.NON_US_PERSON_DECLARATION,
                TAX_RESIDENT_COUNTRY = onboardingData.TAX_RESIDENT_COUNTRY,
                TIN_NO = onboardingData.TIN_NO,
                REASON_FOR_NO_TIN_NUMBER = onboardingData.REASON_FOR_NO_TIN_NUMBER,
                HAVE_TIN = onboardingData.HAVE_TIN,
                CRS_DECLARATION = onboardingData.CRS_DECLARATION,
                CNIC_UPLOAD_FRONT = onboardingData.CNIC_UPLOAD_FRONT,
                CNIC_UPLOAD_BACK = onboardingData.CNIC_UPLOAD_BACK,
                LIVEPHOTE_OR_SELFIE = onboardingData.LIVEPHOTE_OR_SELFIE,
                PROOF_OF_SOURCE_OF_INCOME = onboardingData.PROOF_OF_SOURCE_OF_INCOME,
                PAST_ONE_YEAR_BANKSTATEMENT = onboardingData.PAST_ONE_YEAR_BANKSTATEMENT,
                FAMILY_MEMBER = onboardingData.FAMILY_MEMBER,
                COMPANY_REGISTER = onboardingData.COMPANY_REGISTER,
                INTERNATIONAL_NUMBER = onboardingData.INTERNATIONAL_NUMBER
            };

            //  Excel file generate DTO
            var filePath = SaveExcelFileToDisk(dto, dropdowns, riskProfileQuestions);

            if (File.Exists(filePath))
            {
                builder.Attachments.Add(filePath);
            }

            message.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                _logger.LogInformation("Connecting to SMTP...");

                await smtp.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("✅ Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Email send failed: {Message}", ex.Message);
                throw;
            }
        }


        private string SaveExcelFileToDisk(AccountOpeningDTO data, Dictionary<string, List<DropDownDTO>> dropdowns, List<QuestionnaireItemDTO> riskProfileQuestions)
        {
            string folderPath = @"D:\ExcelFileOnboarding";
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"OnboardingDetails_{data.ACCOUNTID}.xlsx");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Merge risk profile dropdowns into main dropdowns
            foreach (var item in riskProfileQuestions)
            {
                if (!string.IsNullOrEmpty(item.Key) && item.Answers != null && !dropdowns.ContainsKey(item.Key))
                {
                    dropdowns[item.Key] = item.Answers;
                }
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Onboarding Details");

                var properties = typeof(AccountOpeningDTO).GetProperties();

                // ✅ Property → Dropdown key mapping
                var dropdownMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "BANK_NAME", "banks" },
            { "PERMANENT_CITY", "cities" },
            { "MAILING_CITY", "cities" },
            { "PERMANENT_COUNTRY", "countries" },
            { "MAILING_COUNTRY", "countries" },
            { "SOURCE_OF_INCOME", "sourceOfIncomes" },
            { "OCCUPATION", "occupations" },
            { "PROFESSION", "profession" },
            { "WALLET_NAME", "wallet" },
            { "EXPECTED_MONTHLY_INVESTEMENTAMOUNT", "expectedInvestments" },
            { "EXPECTED_MONTHLY_NO_OF_REDEMPTION_TRANSACTION", "expectedNoOfRedemptions" },
            { "MOBILE_NUMBER_OWNERSHIP", "mobileOwnerships" },
            { "REASON_FOR_NO_TIN_NUMBER", "noTINReasons" },
            { "EXPECTED_INVESTMENT_AMOUNT", "expectedNoInvestment" },
            { "EXPECTED_MONTHLY_NO_OF_INVESTMENT_TRANSACTION", "expectedMonthlyNoOfInvestmentTransaction" },

                // Risk Profile dropdowns
                    { "age", "age" },
                    { "MARITAL_STATUS", "maritalStatus" },
                    { "NO_OF_DEPENDENTS", "noOfDependents" },
                    { "Education", "education" },
                    { "RISK_APPETITE", "riskAppetite" },
                    { "INVESTMENT_OBJECTIVE", "investmentObjective" },
                    { "INVESTMENT_HORIZON", "investmentHorizon" },
                    { "INVESTMENT_KNOWLEDGE", "investmentKnowledge" },
                    { "FINANCIAL_POSITION", "financialPosition" }
        };

                // ✅ Yes/No wali fields
                var yesNoFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "NON_US_PERSON_0DECLARATION",
            "INSTRUCTIONS_TO_TRANSFER_FUNDS_TO_ACCOUNT_MAINTAINED_IN_USA",
            "HAVE_US_RESIDENCE_MAILING_HOLDMAILING_ADDRESS",
            "HAVE_US_TELEPHONE_NUMBER",
            "FATCA_DECLARATION",
            "NON_US_PERSON_DECLARATION",
            "HAVE_TIN",
            "CRS_DECLARATION",
            "IS_US_CITIZEN_RESIDENT_HAVEGREENCARD",
            "INTERNATIONAL_RELATION_OR_PEP",
            "FINANCIAL_INSTITUUTION_REFUSAL",
            "ULTIMATE_BENEFICIARY",
            "HOLDING_SENIOR_POSITION",
            "DEAL_WITH_HIGH_VALUE_ITEMS",
            "FINANCIAL_LINKS_TO_OFFSHORE_TAX_HAVENS",
            "TRUE_INFORMATION_DECLARATION",
            "RISK_PROFILE_DECLARATION",
            "IS_MAILING_SAME_AS_PERMANENT_ADDRESS",
            "PRINCIPLE_CNIC_EXP_LIFE_TIME",
            "BASICINFO_STEP1",
            "BASICINFO_STEP2",
            "PERSONALDETAILS_STEP1",
            "PERSONALDETAILS_STEP2",
            "PERSONALDETAILS_STEP3",
            "RISKPROFILE_STEP",
            "REGULATORYKYC_STEP1",
            "REGULATORYKYC_STEP2",
            "REGULATORYKYC_STEP3",
            "REGULATORYKYC_STEP4",
            "UPLOADDOCUMENT_STEP",
            "US_BORN"
        };

                int col = 1;
                foreach (var prop in properties)
                {
                    worksheet.Cell(1, col).Value = prop.Name;

                    var value = prop.GetValue(data);

                    if (value != null && dropdownMap.TryGetValue(prop.Name, out var dropdownKey))
                    {
                        // ID ko dropdown Title me convert karna
                        if (dropdowns.TryGetValue(dropdownKey, out var options))
                        {
                            var intValue = Convert.ToInt32(value);
                            var title = options.FirstOrDefault(x => x.Id == intValue)?.Title;
                            worksheet.Cell(2, col).Value = title ?? value.ToString();
                        }
                        else
                        {
                            worksheet.Cell(2, col).Value = value.ToString();
                        }
                    }
                    else if (value is byte[] byteArray)
                    {
                        //Image insert 
                        try
                        {
                            using (var stream = new MemoryStream(byteArray))
                            {
                                var picture = worksheet.AddPicture(stream)
                                                       .MoveTo(worksheet.Cell(2, col));

                                picture.Width = 150;
                                picture.Height = 150;

                                worksheet.Column(col).Width = 22;
                                worksheet.Row(2).Height = 110;
                            }
                        }
                        catch
                        {
                            worksheet.Cell(2, col).Value = Convert.ToBase64String(byteArray);
                        }
                    }
                    else if (value is int intVal && yesNoFields.Contains(prop.Name) && (intVal == 0 || intVal == 1))
                    {
                        worksheet.Cell(2, col).Value = intVal == 1 ? "Yes" : "No";
                    }
                    else
                    {
                        worksheet.Cell(2, col).Value = value?.ToString() ?? string.Empty;
                    }

                    col++;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }

            return filePath;
        }


    }
}
