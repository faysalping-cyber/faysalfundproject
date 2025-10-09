using FaysalFundsInternal.CrossCutting.Responses;
using FluentValidation;

//using System.ComponentModel.DataAnnotations;
//using System.DirectoryServices.Protocols;
using System.Net;
using System.Text.Json;

namespace FaysalFundsInternal.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to the next middleware
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                var response = ApiResponse<object>.Failure(ex.Message);
                
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await context.Response.WriteAsync(json);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                var response = ApiResponse<object>.Failure(ex.Message);

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await context.Response.WriteAsync(json);
            }

        }
        //private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        //{
        //    var message = string.Join("; ", exception.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
        //    var responseObj = ApiResponse<object>.Failure(message);
        //    var responseJson = JsonSerializer.Serialize(responseObj, _jsonOptions);

        //    await WriteJsonResponseAsync(context, HttpStatusCode.UnprocessableEntity, responseJson);
        //}
        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            var message = string.Join("; ", exception.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            var responseObj = ApiResponse<object>.Failure(message);
            var responseJson = JsonSerializer.Serialize(responseObj, _jsonOptions);

            await WriteJsonResponseAsync(context, HttpStatusCode.UnprocessableEntity, responseJson);
        }
        private static async Task WriteJsonResponseAsync(HttpContext context, HttpStatusCode statusCode, string json)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(json);
        }
    }
}
