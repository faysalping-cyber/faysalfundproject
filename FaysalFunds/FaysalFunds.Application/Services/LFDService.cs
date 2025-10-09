using FaysalFunds.Application.DTOs;
using FaysalFunds.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace FaysalFunds.Application.Services
{
    public class LFDService
    {
        private readonly LFD_Credentials _lFD_Credentials;
        private readonly LFD_Settings _lFD_Settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string _loveForData = "LoveForDataAPi";
        private readonly string _TimeStamp = "2019-11-25T10:55:51Z";

        public LFDService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _lFD_Credentials = configuration.GetSection("LFD_Credentials").Get<LFD_Credentials>();
            _lFD_Settings = configuration.GetSection("LFD_Settings").Get<LFD_Settings>();
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SearchResponseModel> Search(string cnic)
        {
            var authTokenResponse = await GetAuthToken();
            var signature = await GetSignature(cnic);

            HttpClient client = _httpClientFactory.CreateClient(_loveForData);
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri("https://hybridapi.lfdanalytics.com/api/search/"),
                Method = HttpMethod.Post
            };

            // Set headers before adding content
            //message.Headers.Add("Authorization", "jwt " + authTokenResponse.Result);
            //message.Headers.Add("Authorization", "jwt " + authTokenResponse);
            message.Headers.TryAddWithoutValidation("Authorization", "jwt " + authTokenResponse);

            message.Headers.Add("Signature", signature.Data);
            message.Headers.Add("Key", _lFD_Credentials.UserKey);
            message.Headers.Add("TimeStamp", _TimeStamp);
            message.Headers.Add("Pragma", "no-cache");
            message.Headers.Add("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");

            // Create search model with a local UUID to avoid race conditions
            string uuid = GenerateUniqueId();
            SearchModel searchModel = new()
            {
                uuid = uuid,
                cnic = cnic,
                type = _lFD_Settings.Type,
                tier = _lFD_Settings.Tier,
            };

            message.Content = new StringContent(JsonConvert.SerializeObject(searchModel), Encoding.UTF8, "application/json");

            HttpResponseMessage apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<SearchResponseModel>(apiContent);
            return deserializedResponse;
        }

        private async Task<ApiResponseWithData<string>> GetAuthToken()
        {

            HttpClient client = _httpClientFactory.CreateClient(_loveForData);
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri("https://hybridapi.lfdanalytics.com/api-token-auth/"),
                Method = HttpMethod.Post
            };

            message.Headers.Add("Pragma", "no-cache");
            message.Headers.Add("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");

            // Send credentials as form-data
            var formData = new MultipartFormDataContent
                {
                    { new StringContent(_lFD_Credentials.Username), "username" },
                    { new StringContent(_lFD_Credentials.Password), "password" }
                };

            message.Content = formData;
            HttpResponseMessage apiResponse = await client.SendAsync(message);


            if (apiResponse.IsSuccessStatusCode)
            {
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<AuthResponseModel>(apiContent);
                if (deserializedResponse is null)
                {
                    return ApiResponseWithData<string>.FailureResponse($"LFD Auth Tokex: Something went wrong while deserializing");

                }
                else
                {
                    return ApiResponseWithData<string>.SuccessResponse(deserializedResponse.token);
                }
            }
            else
            {
                return ApiResponseWithData<string>.FailureResponse($"LFD Signature: {apiResponse.ReasonPhrase}");
            }

        }

        private async Task<ApiResponseWithData<string>> GetSignature(string cnic)
        {

            HttpClient client = _httpClientFactory.CreateClient(_loveForData);

            string uuid = GenerateUniqueId();
            var request = new SignatureModel()
            {
                auth_key = _lFD_Credentials.AuthKey,
                data = new RequestData { cnic = cnic, tier = _lFD_Settings.Tier, type = _lFD_Settings.Type, uuid = uuid },
                ip_address = "111.88.99.232",
                request_method = "POST",
                secret_key = _lFD_Credentials.SecretKey,
                time_stamp = _TimeStamp,
                url_path = "/api/search/"
            };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri("https://hmac-sign-api.lfdanalytics.com/api/get_hmac_signature"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage apiResponse = await client.SendAsync(message);
            var apiContent = await apiResponse.Content.ReadAsStringAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                var successResponse = JsonConvert.DeserializeObject<SignatureResponseModel>(apiContent);
                if (!String.IsNullOrEmpty(successResponse.error))
                {
                    return ApiResponseWithData<string>.FailureResponse($"LFD Signature: {successResponse.error}");
                }
                return ApiResponseWithData<string>.SuccessResponse(successResponse.data);
            }
            else
            {
                return ApiResponseWithData<string>.FailureResponse($"LFD Signature: {apiResponse.ReasonPhrase}");
            }
        }

        private static string GenerateUniqueId()
        {
            string currentDate = DateTime.Now.ToString("ddMMyy");
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", "")
                .Replace("+", "")
                .Replace("/", "")
                .Substring(0, 5) + currentDate;
        }
    }
}
