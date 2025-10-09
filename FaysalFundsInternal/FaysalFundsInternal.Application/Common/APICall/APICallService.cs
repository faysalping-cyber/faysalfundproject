using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace FaysalFundsInternal.Common
{
    public class APICallService : IAPICallService
    {
        private readonly HttpClient _httpClient;
        public APICallService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //public async Task<string> PostDataAsync<T>(string endpoint, T data, Dictionary<string, string> headers = null)
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, Dictionary<string, string> headers = null)
        {
            //var json = JsonSerializer.Serialize(data);
            //// Add headers if provided
            //if (headers != null)
            //{
            //    foreach (var header in headers)
            //    {
            //        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            //    }
            //}
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            //var response = await _httpClient.PostAsync(url, content);
            //string stringResponse = await response.Content.ReadAsStringAsync();
            //string code = GetPropertyFromJSONResponse(stringResponse, "ResponseCode");
            //if (code == "0" || code == "")
            //{
            //    response.EnsureSuccessStatusCode();
            //    return await response.Content.ReadAsStringAsync();
            //}
            //else
            //{
            //    string errorDescription = GetPropertyFromJSONResponse(stringResponse, "Description");
            //    throw new ApiException(ApiErrorCodes.BadRequest, errorDescription);
            //}
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(data)
            };
            AddHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();

        }

        async Task<T> IAPICallService.GetAsync<T>(string endpoint, Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            AddHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private string GetPropertyFromJSONResponse(string response, string propertyName)
        {
            string propertyValue = "";
            using (JsonDocument doc = JsonDocument.Parse(response))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty(propertyName, out JsonElement val))
                {
                    propertyValue = val.GetString();
                    Console.WriteLine(propertyValue);
                }
            }
            return propertyValue;
        }
        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

        }

    }
}