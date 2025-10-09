using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.DTOs.ExternalAPI;
using FaysalFunds.Application.DTOs.TransactionAllowedDTO;
using FaysalFunds.Common;
using FaysalFunds.Common.APIException;
using FaysalFunds.Common.ApiResponses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FaysalFunds.Infrastructure.ExternalService
{
    public class FamlInternalService
    {
        private readonly BaseUrls _baseUrls;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FamlInternalService(HttpClient httpClient, IConfiguration configuration, IOptions<BaseUrls> baseUrls)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrls = baseUrls.Value;
        }
        public async Task<List<CheckBalanceResponseModel>> CheckBalance(CheckBalanceRequestModel request) 
        {
            var url = _configuration["BaseUrls/FaysalInternal"];

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = await JsonSerializer.DeserializeAsync<List<CheckBalanceResponseModel>>(responseStream, options);

            return result ?? new List<CheckBalanceResponseModel>();
        }

        public async Task<ApiResponseWithData<CheckBalance>> CheckCustomerBalance(CheckBalanceRequestModel request)
        {
            var url = _baseUrls.FaysalInternal + "/AccountDetails/CheckBalance";

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseStream = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                var err = await JsonSerializer.DeserializeAsync<ApiResponseNoData>(responseStream, options);
                throw new ApiException(err.Message);
            }

            decimal totalBalance = 0;

            var result = await JsonSerializer.DeserializeAsync<ApiResponseWithData<List<CheckBalanceResponseModel>>>(responseStream, options);
            foreach (var item in result.Data)
            {
                totalBalance = totalBalance + item.BalanceAmount;
            }
            CheckBalance checkBalance = new CheckBalance
            {
                TotalBalance = totalBalance,
                CheckBalanceList = result.Data
            };
            return ApiResponseWithData<CheckBalance>.SuccessResponse(checkBalance);
        }

        public async Task<ApiResponseWithData<ProfitResponseModel>> CheckCustomerProfit(ProfitRequestModel request)
        {
            string url = _baseUrls.FaysalInternal + "/AccountDetails/GetSumOfProfit";

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseStream = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                var err = await JsonSerializer.DeserializeAsync<ApiResponseNoData>(responseStream, options);
                throw new ApiException(err.Message);
            }
            var result = await JsonSerializer.DeserializeAsync<ApiResponseWithData<ProfitResponseModel>>(responseStream, options);
            return result;
        }


        //generate KuickpayIO
        public async Task<ApiResponseWithData<KuickPayFinalResponse>> GenerateKuickPayId(GenerateKuickPayIdDTO request)
        {
            if (request.Mobile.StartsWith("92"))
            {
                request.Mobile = "0" + request.Mobile.Substring(2);
            }
            else
            {
                throw new ApiException("Number must be from Pakistan and start with '92'.");
            }
            string url = _baseUrls.FaysalInternal + "/AccountDetails/GenerateKuickPayID";

            //var demo = "https://localhost:7201/api";
            //string url = demo + "/AccountDetails/GenerateKuickPayID";

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var responseStream = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                var err = await JsonSerializer.DeserializeAsync<ApiResponseNoData>(responseStream, options);
                throw new ApiException(err.Message);
            }

            var result = await JsonSerializer.DeserializeAsync<KuickPayResponseModel>(responseStream, options);
            return ApiResponseWithData<KuickPayFinalResponse>.SuccessResponse(new KuickPayFinalResponse

            {
                ResponseCode = result.response_Code,
                KuickPayId = result.response_description, 
                ResponseDetail = result.responseCode_Detail
            });
        }



    }
}
