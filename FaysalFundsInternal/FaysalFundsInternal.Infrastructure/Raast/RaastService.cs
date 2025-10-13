using FaysalFundsInternal.CrossCutting.Responses;
using FaysalFundsInternal.Infrastructure.DTOs.Raast;
using Newtonsoft.Json;
using System.Text;

namespace FaysalFundsInternal.Infrastructure.Raast
{
    public class RaastService : IRaastService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _Raast = "RaastAPI";

        public RaastService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResponse<GenerateIbanResponse>> GenerateIban(GenerateIbanRequestModel request)
        {
            request.Channel = await GetEncryptedChannelId();

            var client = _httpClientFactory.CreateClient(_Raast);

            using var message = new HttpRequestMessage(HttpMethod.Post,
                "https://uatsvr.faysalfunds.com:8006/uat/raastserver/GenerateIBAN")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            var apiResponse = await client.SendAsync(message);
            var apiContent = await apiResponse.Content.ReadAsStringAsync();

            if (!apiResponse.IsSuccessStatusCode)
                throw new ApiException($"Raast GenerateIBAN failed. Status: {(int)apiResponse.StatusCode} {apiResponse.ReasonPhrase}, Response: {apiContent}");

            var deserializedResponse = JsonConvert.DeserializeObject<GenerateIbanResponseModel>(apiContent);
            if (deserializedResponse == null)
                throw new ApiException("Invalid or empty response received from Raast GenerateIBAN API.");
            if (deserializedResponse.ResponseCode == "99")
            {
                throw new ApiException(deserializedResponse.ResponseMessage);
            }
            var responseModel = new GenerateIbanResponse
            {
                RaastIban = deserializedResponse.RaastIBAN
            };
            return ApiResponse<GenerateIbanResponse>.Success(responseModel);
        }


        public async Task<ApiResponse<List<IbanListModel>>> GetIbanList(ListIbanRequestModel request)
        {
            request.ChannelID = await GetEncryptedChannelId();

            HttpClient client = _httpClientFactory.CreateClient(_Raast);

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri("https://uatsvr.faysalfunds.com:8006/uat/raastserver/ListIBAN"),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json")
            };

            // ✅ Send the request
            var response = await client.SendAsync(message);
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode && json.TrimStart().StartsWith("["))
            {
                // ✅ Success case
                var result = JsonConvert.DeserializeObject<List<IbanListModel>>(json);

                return ApiResponse<List<IbanListModel>>.Success(result);
            }
            else
            {
                // ❌ Failure case
                var error = JsonConvert.DeserializeObject<ListIbanErrorResponse>(json);
                throw new Exception(error?.ResponseMessage ?? "API call failed.");
            }
        }


        private async Task<string> GetEncryptedChannelId()
        {
            string channelId = "0549cd67-8229-49cb-8235-2329d7ab4bd6";
            HttpClient client = _httpClientFactory.CreateClient(_Raast);
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri("https://inhouse.faysalfunds.com:201/secure/security/SecureKeyAPI"),
                Method = HttpMethod.Post,
                
            };
            SecureKeyRequestModel request = new() { UserData= channelId };
            message.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage apiResponse = await client.SendAsync(message);
            var apiContent = await apiResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<SecureKeyResponseModel>(apiContent);
            return result.HashKey;
        }
    }
}
