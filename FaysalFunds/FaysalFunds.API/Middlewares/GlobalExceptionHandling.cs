using FaysalFunds.API.Models;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace FaysalFunds.API.Middlewares
{
    public class GlobalExceptionHandling : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandling> _logger;
        private readonly string _logFilePath;
        private readonly string dateTimeFormat = "d/M/yyyy h:mm:ss tt";

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
            catch (ApiException ex)
            {
                await HandleApiException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleApiException(HttpContext context, ApiException exception)
        {
            var errorResponse = ApiResponseNoData.FailureResponse(exception.Message,exception.ErrorHeading,exception.ErrorIcon);

            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            context.Response.ContentType = "application/json";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false // Optional: set to true for pretty-print
            };
            string responseJson = JsonSerializer.Serialize(errorResponse,options);
            await context.Response.WriteAsync(responseJson);
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string errorId = Guid.NewGuid().ToString();
            string timestamp = DateTime.Now.ToString(dateTimeFormat, CultureInfo.InvariantCulture);

            string errorMessage = $"An unexpected error occurred. Please contact support with this Error ID: {errorId}.";
            //var responseObj = new ErrorCodeResponse(ApiErrorCodes.InternalError, errorMessage, errorId);
            var responseObj = ApiResponseNoData.FailureResponse(errorMessage);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false // Optional: set to true for pretty-print
            };
            string responseJson = JsonSerializer.Serialize(responseObj,options);

            // Log to logger
            _logger.LogError(exception, "Error ID: {ErrorId} - {Message}", errorId, exception.Message);

            // Write to CSV file
            string logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            if (!File.Exists(_logFilePath))
            {
                string header = "Date,ErrorId,ErrorMessage,InnerException\n";
                await File.WriteAllTextAsync(_logFilePath, header);
            }

            var innerExceptionMsg = exception.InnerException?.Message ?? "";
            string csvLine = FormatCsvLine(timestamp, errorId, exception.Message, innerExceptionMsg);
            await File.AppendAllTextAsync(_logFilePath, csvLine);

            // Return error response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await context.Response.WriteAsync(responseJson);
        }

        private string FormatCsvLine(params string[] values)
        {
            var formattedValues = values.Select(v => $"\"{v?.Replace("\"", "\"\"")}\"");
            return string.Join(",", formattedValues) + "\n";
        }

        //private int MapErrorCodeToStatusCode(int errorCode)
        //{
        //    return errorCode switch
        //    {
        //        ApiErrorCodes.BadRequest => StatusCodes.Status400BadRequest,
        //        ApiErrorCodes.Unauthorized => StatusCodes.Status401Unauthorized,
        //        ApiErrorCodes.NotFound => StatusCodes.Status404NotFound,
        //        ApiErrorCodes.Forbidden => StatusCodes.Status403Forbidden,
        //        _ => StatusCodes.Status500InternalServerError
        //    };
        //}
    }
}
