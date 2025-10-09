using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Middleware;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Services;
using FaysalFundsWrapper.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddHttpContextAccessor();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<TokenProviderService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJWTBlackLisService, JWTBlackLisService>();
builder.Services.AddScoped<IOnBoardingService, OnBoardingService>();
builder.Services.AddScoped<ITransactionPinService, TransactionPinService>();
builder.Services.AddScoped<IDashhboardServices, DashhboardServices>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<ContactDetailValidation>();


// Middleware
builder.Services.AddTransient<GlobalExceptionHandling>();
// JWT Authentication Setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            //ValidIssuer = builder.Configuration["Jwt:Issuers"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            //ClockSkew = TimeSpan.Zero
            ValidateIssuer = true,//
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.FallbackPolicy = null; // This ensures [AllowAnonymous] works
});

// HttpClient for Main API
builder.Services.AddHttpClient<IMainApiClient, MainApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MainApi:BaseUrl"]);
    client.DefaultRequestHeaders.Add("x-api-key", builder.Configuration["MainApi:ApiKey"]);
});
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
        var response = ApiResponseNoData.FailureResponse(message: errorMessage);

        return new UnprocessableEntityObjectResult(response);
    };
});


var app = builder.Build();


app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        response.ContentType = "application/json";
        var result = ApiResponseNoData.FailureResponse("Unauthorized access. Please log in.");
        await response.WriteAsync(JsonSerializer.Serialize(result));
    }
    else if (response.StatusCode == StatusCodes.Status403Forbidden)
    {
        response.ContentType = "application/json";
        var result = ApiResponseNoData.FailureResponse("Forbidden: You don't have permission.");
        await response.WriteAsync(JsonSerializer.Serialize(result));
    }
});




// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseMiddleware<GlobalExceptionHandling>();

//app.UseHttpsRedirection();

//app.UseAuthentication(); // Built-in JWT token validation

//app.UseMiddleware<JwtMiddleware>(); // Blacklist check (AFTER authentication)

//app.UseAuthorization(); // Checks [Authorize] attributes

//app.MapControllers();



app.UseMiddleware<GlobalExceptionHandling>();

app.UseStatusCodePages(); // must be early

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();