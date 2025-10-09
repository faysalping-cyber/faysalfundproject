using FaysalFundsWrapper.Interfaces;
using System.Text.Json;
using System.Text;
using FaysalFundsWrapper.Models;

namespace FaysalFundsWrapper.Services
{
    public class MainApiClient : IMainApiClient
    {
        private readonly HttpClient _httpClient;

        public MainApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<string> GetAsync(string endpoint)
        //{
        //    var response = await _httpClient.GetAsync(endpoint);
        //    response.EnsureSuccessStatusCode();

        //    return await response.Content.ReadAsStringAsync();
        //}

        //public async Task<string> PostAsync<TRequest>(string endpoint, TRequest data)
        //{
        //    var json = JsonSerializer.Serialize(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(endpoint, content);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}
        public async Task<string> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                // Optional: log or deserialize custom error model here
                throw new HttpRequestException($"Request failed: {response.StatusCode}, Response: {responseContent}");
            }
            return responseContent;
        }

        //public async Task<string> PostAsync<TRequest>(string endpoint, TRequest data)
        //{
        //    var json = JsonSerializer.Serialize(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(endpoint, content);
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        //throw new HttpRequestException($"Request failed: {response.StatusCode}, Response: {responseContent}");
        //        var error = JsonSerializer.Deserialize<ApiResponseNoData>(responseContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });
        //        throw new ApiException(error.Message,error.ErrorHeading);
        //    }

        //    return responseContent;
        //}
        public async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var error = JsonSerializer.Deserialize<ApiResponseNoData>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    });

                    throw new ApiException(error?.Message ?? "Request failed", error?.ErrorHeading ?? "Error");
                }
                catch
                {
                    throw new HttpRequestException($"Request failed: {response.StatusCode}, Response: {responseContent}");
                }
            }

            var result = JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            return result!;
        }

        public async Task<string> PostAsync<TRequest>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<ApiResponseNoData>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                throw new ApiException(error.Message, error.ErrorHeading);
            }

            return responseContent; // Do NOT deserialize/serialize again
        }

        //public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        //{
        //    var json = JsonSerializer.Serialize(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(endpoint, content);
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var error = JsonSerializer.Deserialize<ApiResponseNoData>(responseContent, new JsonSerializerOptions
        //        {
        //            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //        });
        //        throw new ApiException(error.Message, error.ErrorHeading);
        //    }
        //    var  df =JsonSerializer.Deserialize<TResponse>(responseContent
        //    return JsonContent.Create( df);
        //}
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<ApiResponseNoData>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
                throw new ApiException(error?.Message ?? "Unknown error",error.ErrorHeading,error.ErrorIcon);
            }

            var result = JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            return result!;
        }

    }
}
