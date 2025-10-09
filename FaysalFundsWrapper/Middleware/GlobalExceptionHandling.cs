using FaysalFundsWrapper.Models;
using FluentValidation;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace FaysalFundsWrapper.Middleware
{
    public class GlobalExceptionHandling : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandling> _logger;
        private readonly string _logFilePath;
        private const string DateTimeFormat = "d/M/yyyy h:mm:ss tt";

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public GlobalExceptionHandling(ILogger<GlobalExceptionHandling> logger)
        {
            _logger = logger;
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "error_logs.csv");
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (ApiException ex)
            {
                await HandleApiExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnhandledExceptionAsync(context, ex);
            }
        }

        private async Task HandleUnhandledExceptionAsync(HttpContext context, Exception exception)
        {
            string errorId = Guid.NewGuid().ToString();
            string timestamp = DateTime.Now.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

            _logger.LogError(exception, "Unhandled Exception | Error ID: {ErrorId}", errorId);

            var errorMessage = $"An unexpected error occurred. Please contact support with this Error ID: {errorId}";
            var responseObj = ApiResponseNoData.FailureResponse(errorMessage);
            var responseJson = JsonSerializer.Serialize(responseObj, _jsonOptions);

            EnsureLogFileExists();
            await LogErrorToCsvAsync(timestamp, errorId, exception.Message, exception.InnerException?.Message ?? string.Empty);

            await WriteJsonResponseAsync(context, HttpStatusCode.UnprocessableEntity, responseJson);
        }

        private async Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
        {
            _logger.LogWarning("API Exception: {Message}", exception.Message);

            var responseObj = ApiResponseNoData.FailureResponse(exception.Message,exception.ErrorHeading,exception.ErrorIcon);
            var responseJson = JsonSerializer.Serialize(responseObj, _jsonOptions);

            await WriteJsonResponseAsync(context, (HttpStatusCode)responseObj.Code, responseJson);
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            var message = string.Join("; ", exception.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            var responseObj = ApiResponseNoData.FailureResponse(message);
            var responseJson = JsonSerializer.Serialize(responseObj, _jsonOptions);

            await WriteJsonResponseAsync(context, HttpStatusCode.UnprocessableEntity, responseJson);
        }

        private static async Task WriteJsonResponseAsync(HttpContext context, HttpStatusCode statusCode, string json)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(json);
        }

        private void EnsureLogFileExists()
        {
            var logDirectory = Path.GetDirectoryName(_logFilePath)!;

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            if (!File.Exists(_logFilePath))
            {
                var header = "Date,ErrorId,ErrorMessage,InnerException\n";
                File.WriteAllText(_logFilePath, header);
            }
        }

        private async Task LogErrorToCsvAsync(string timestamp, string errorId, string message, string innerMessage)
        {
            var line = FormatCsvLine(timestamp, errorId, message, innerMessage);
            await File.AppendAllTextAsync(_logFilePath, line);
        }

        private static string FormatCsvLine(params string[] values)
        {
            var formatted = values.Select(v => $"\"{v?.Replace("\"", "\"\"")}\"");
            return string.Join(",", formatted) + "\n";
        }
    }
}
