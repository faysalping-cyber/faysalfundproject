using FaysalFundsInternal.API.Middlewares;
using FaysalFundsInternal.Application.DTOs;
using FaysalFundsInternal.Application.Services;
using FaysalFundsInternal.Common;
using FaysalFundsInternal.CrossCutting.Constants;
using FaysalFundsInternal.CrossCutting.Responses;
using FaysalFundsInternal.Domain.Interfaces;
using FaysalFundsInternal.Infrastructure;
using FaysalFundsInternal.Infrastructure.DTOs;
using FaysalFundsInternal.Infrastructure.Raast;
using FaysalFundsInternal.Persistence;
using FaysalFundsInternal.Persistence.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
//services
builder.Services.AddScoped<IAccountStatementRepository, AccountStatementRepository>();
builder.Services.AddScoped<IFamlFundDetailRepository, FamlFundDetailRepository>();
builder.Services.AddScoped<ILFDService, LFDService>();
builder.Services.AddScoped<IRaastService, RaastService>();
//Repos
builder.Services.AddScoped<ILFDService, LFDService>();
builder.Services.AddScoped<AccountStatementService>();
builder.Services.AddScoped<IUHRRepository, UHRRepository>();
builder.Services.AddScoped<IUHSRepository, UHSRepository>();
builder.Services.AddScoped<IUnitHolderRepository, UnitHolderRepository>();
builder.Services.AddScoped<KuickPayService>();
builder.Services.AddHttpClient("LoveForDataAPi", client =>
{
    client.BaseAddress = new Uri("https://hybridapi.lfdanalytics.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.Configure<Constants>(builder.Configuration.GetSection("Constants"));
builder.Services.AddHttpClient<IAPICallService, APICallService>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // Get error messages from the model state
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        // Join errors into a single string
        var errorMessage = string.Join(" | ", errors);

        // Return your custom response model
        var response = ApiResponse<object>.Failure(errorMessage);

        return new UnprocessableEntityObjectResult(response);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<BlockedAccountTypes>(builder.Configuration.GetSection("BlockedAccountTypes"));
builder.Services.Configure<LFD_Credentials>(builder.Configuration.GetSection("LFD_Credentials"));
builder.Services.Configure<LFD_Settings>(builder.Configuration.GetSection("LFD_Settings"));
var app = builder.Build();
//kuickpay code
app.MapGet("/", (IConfiguration configuration) =>
{
    var constants = configuration.GetSection("Constants").Get<Constants>();
    return $"Normal: {constants.Normal}, Individual: {constants.Individual}, InstitutionId: {constants.InstitutionId},KuickPayUsername:{constants.KuickPayUserName},KuickPayPassword:{constants.KuickPayPassword}";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
