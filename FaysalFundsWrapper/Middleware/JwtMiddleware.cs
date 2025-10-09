using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using System.Text.Json;

namespace FaysalFundsWrapper.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Check for blacklisted token (only if token exists and user is authenticated)
            if (!string.IsNullOrEmpty(token) && context.User.Identity?.IsAuthenticated == true)
            {
                var isBlackListed = await IsTokenBlackListed(token, context);
                if (isBlackListed)
                {
                    await WriteUnauthorizedResponse(context, "Unauthorized access. Please login.");
                    return;
                }
            }

            await _next(context);

            // If token is expired or invalid, this block catches the 401
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized && !context.Response.HasStarted)
            {
                await WriteUnauthorizedResponse(context, "Session expired. Please login again.");
            }
        }

        private async Task<bool> IsTokenBlackListed(string token, HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<IJWTBlackLisService>();
            var response = await authService.IsTokenBlacklistedAsync(token);
            var result = JsonSerializer.Deserialize<ApiResponseWithData<bool>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return result?.Data ?? false;
        }

        private async Task WriteUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = ApiResponseNoData.FailureResponse(message);
            response.Code = StatusCodes.Status401Unauthorized;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
