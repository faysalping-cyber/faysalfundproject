using FaysalFunds.API.Middlewares;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.Services;
using FaysalFunds.Application.Services.IServices;
using FaysalFunds.Application.Utilities;
using FaysalFunds.Common;
using FaysalFunds.Domain.Interfaces;
using FaysalFunds.Domain.Interfaces.Dropdowns;
using FaysalFunds.Infrastructure.ExternalService;
using FaysalFunds.Persistence.Database;
using FaysalFunds.Persistence.Repositories;
using FaysalFunds.Persistence.Repositories.Dropdowns;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Register Services ---

// Middleware
builder.Services.AddTransient<GlobalExceptionHandling>();

// Application Services
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<LFDService>();
builder.Services.AddScoped<UHSService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<JWTBlackListService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<DropdownService>();
builder.Services.AddScoped<AccountOpeningService>();
builder.Services.AddScoped<FamlFundsService>();
builder.Services.AddScoped<RiskProfileService>();
builder.Services.AddScoped<ProfileScoreService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<TransactionPinService>();
builder.Services.AddScoped<DashhboardServices>();
builder.Services.AddScoped<KuickPayServices>();
builder.Services.AddScoped<SmsService>();
builder.Services.AddScoped<FamlInternalService>();



//builder.Services.AddSingleton<>();

// HTTP Client
builder.Services.AddHttpClient();

// Repository Layer
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IUHSRepository, UHSRepository>();
builder.Services.AddScoped<IUserPasswordHistoryRepository, UserPasswordHistoryRepository>();
builder.Services.AddScoped<IJWTBlackListRepository, JWTBlackListRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAccountOpeningRepository, AccountOpeningRepository>();
builder.Services.AddScoped<IFamlFundRepository, FamlFundRepository>();
builder.Services.AddScoped<ITransactionPinRepository, TpinTransactionRepository>();

//Dropdown Repository
builder.Services.AddScoped<IAgeRepository, AgeRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IExpectedInvestmentRepository, ExpectedInvestmentRepository>();
builder.Services.AddScoped<IExpectedNOofRedumptionRepository, ExpectedNOofRedumptionRepository>();
builder.Services.AddScoped<IFinancialPositionRepository, FinancialPositionRepository>();
builder.Services.AddScoped<IInvestmentHorizonRepository, InvestmentHorizonRepository>();
builder.Services.AddScoped<IInvestmentKnowledgeRepository, InvestmentKnowledgeRepository>();
builder.Services.AddScoped<IInvestmentObjectivesRepository, InvestmentObjectivesRepository>();
builder.Services.AddScoped<IMartialStatusRepository, MartialStatusRepository>();
builder.Services.AddScoped<IMobileOwnershipRepository, MobileOwnershipRepository>();
builder.Services.AddScoped<INoTINReasonRepository, NoTINReasonRepository>();
builder.Services.AddScoped<INumberOfDependentsRepository, NumberOfDependentsRepository>();
builder.Services.AddScoped<IOccupationRepository, OccupationRepository>();
builder.Services.AddScoped<IProfessionRepository, ProfessionRepository>();
builder.Services.AddScoped<IRiskAppititeRepository, RiskAppititeRepository>();
builder.Services.AddScoped<ISourceOfIncomeRepository, SourceOfIncomeRepository>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IProfileScoreRepository, ProfileScoreRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IExpectedInvestmentRepository, ExpectedInvestmentRepository>();
builder.Services.AddScoped<IExpectedMonthlyInvestmentAmountRepository, ExpectedMonthlyInvestmentAmountRepository>();
builder.Services.AddScoped<IExpectedMonthlyNoOfInvestmentTransactionRepository, ExpectedMonthlyNoOfInvestmentTransactionRepository>();
builder.Services.AddScoped<IExpectedMonthlyNoOfRedemptionTransactionRepository, ExpectedMonthlyNoOfRedemptionTransactionRepository>();
builder.Services.AddScoped<IRiskProfileOccupationRepository, RiskProfileOccupationRepository>();
builder.Services.AddScoped<IFamlFundByFAMLRepository, FamlFundByFAMLRepository>();

builder.Services.AddScoped<IQuickAccessRepository, QuickAccessRepository>();
builder.Services.AddScoped<IUserQuickAccessRepository, UserQuickAccessRepository>();
builder.Services.AddScoped<InvesmentFundRepository, InvestmentFundRepository>();
builder.Services.AddScoped<IKpSlabRepository, KpSlabRepository>();
builder.Services.AddScoped<ITransactionTypesGroupRepository, TransactionTypesGroupRepository>();
builder.Services.AddScoped<IFundTransactionGroupRepository, FundTransactionGroupRepository>();
builder.Services.AddScoped<ITransactionFeatureRepository, TransactionFeatureRepository>();
builder.Services.AddScoped<IFundFeaturePermissionRepository, FundFeaturePermissionRepository>();
builder.Services.AddScoped<ITransactionReceiptDetailRepository, TransactionReceiptDetailRepository>();
builder.Services.AddScoped<IInvestmentInstructionRepository, InvestmentInstructionRepository>();
builder.Services.AddSingleton<DateParserUtility>();
//Configuration Registration
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<BaseUrls>(builder.Configuration.GetSection("BaseUrls"));



builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // 50 MB
});
builder.Services.AddScoped<IEmailService, EmailService>();
// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("MW")));

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- Build the App ---
var app = builder.Build();

// --- Middleware Pipeline ---
app.UseMiddleware<GlobalExceptionHandling>();
//app.UseMiddleware<JwtMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// --- API Endpoints ---
app.MapControllers();

// Endpoint to retrieve LFD Credentials
app.MapGet("/LFD_Credentials", (IConfiguration configuration) =>
{
    var credentials = configuration.GetSection("LFD_Credentials").Get<LFD_Credentials>();
    return $"""
        Username: {credentials.Username},
        Password: {credentials.Password},
        SecretKey: {credentials.SecretKey},
        UserKey: {credentials.UserKey},
        AuthKey: {credentials.AuthKey},
        """;
});

// Endpoint to retrieve LFD Settings
app.MapGet("/LFD_Settings", (IConfiguration configuration) =>
{
    var lfdSettings = configuration.GetSection("LFD_Settings").Get<LFD_Settings>();
    return $"""
        Tier: {lfdSettings.Tier},
        Category: {lfdSettings.Category},
        Type: {lfdSettings.Type},
        """;
});

// --- Run the App ---
app.Run();
