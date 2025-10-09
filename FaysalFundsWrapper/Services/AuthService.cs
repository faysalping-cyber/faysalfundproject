using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using System.Text.Json;

namespace FaysalFundsWrapper.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenProviderService _tokenProviderService;
        private readonly string _controller = "account";
        private readonly IMainApiClient _mainApiClient;
        public AuthService(IMainApiClient mainApiClient, TokenProviderService tokenProviderService)
        {
            _mainApiClient = mainApiClient;
            _tokenProviderService = tokenProviderService;
        }

        public async Task<ApiResponseWithData<LoginResponseModel>> LoginAsync(LoginRequest request)
        {
            var response = await _mainApiClient.PostAsync($"{_controller}/login", request);
            var result =await  GetUserDetailWIthAccessToken(response);
            return result;
        }
        public async Task<ApiResponseNoData> SignupAsync(SignupRequest request)
        {
            var response = await _mainApiClient.PostAsync<SignupRequest, ApiResponseNoData>($"{_controller}/signup", request);
            return response;
        }
        public async Task<ApiResponseNoData> ForgotPassword(ForgotPasswordModel request)
        {
            var response = await _mainApiClient.PostAsync<ForgotPasswordModel,ApiResponseNoData>($"{_controller}/ResetPassword", request);
            return response;
        }
        public async Task<ApiResponseNoData> ChangePassword(ChangePasswordModel request)
        {
            var response = await _mainApiClient.PostAsync<ChangePasswordModel, ApiResponseNoData> ($"{_controller}/ChnagePassword", request);
            return response;
        }
        public async Task<ApiResponseNoData> UsersAppAccountVarification(UserDetailsVerification request)
        {
            var response = await _mainApiClient.PostAsync<UserDetailsVerification,ApiResponseNoData>($"{_controller}/UsersAppAccountVarification", request);
            return response;
        }
        public async Task<ApiResponseNoData> Logout(BlackListTokenRequestModel request)
        {
            var response = await _mainApiClient.PostAsync<BlackListTokenRequestModel,ApiResponseNoData>($"{_controller}/logout", request);
            return response;
        }
        public async Task<string> SetPassword(SetPasswordModel request)
        {
            var response = await _mainApiClient.PostAsync($"{_controller}/SetPassword", request);
            return response;
        }
        public async Task<ApiResponseWithData<Dictionary<string, List<DropDownDTO>>>> GetDropdowns()
        {
            var response = await _mainApiClient.GetAsync<ApiResponseWithData<Dictionary<string, List<DropDownDTO>>>>($"{_controller}/GetDropdowns");
            return response;
        }

        private async Task<ApiResponseWithData<LoginResponseModel>> GetUserDetailWIthAccessToken(string loginResponse)
        {
            var deserialized = JsonSerializer.Deserialize<ApiResponseWithData<LoginResponseModel?>>(loginResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Optional: Handle null or error cases
            if (deserialized?.Code == ApiErrorCodes.Failed)
                throw new ApiException(deserialized.Message);
            var userDetails = deserialized.Data;
            // Return access token or any other needed data
            // Generate access token
            var accessToken = _tokenProviderService.Create(new JWTPayload
            {
                Email = userDetails.Email,
                Name = userDetails.Name,
                UserId = userDetails.UserId,
                CNIC = userDetails.Cnic,
                PhoneNo = userDetails.PhoneNo
            });

            // Build response
            var response =  new LoginResponseModel()
                {
                    Name = userDetails.Name,
                    Cnic = userDetails.Cnic,
                    Email = userDetails.Email,
                    CountryCode= userDetails.CountryCode,
                    PhoneNo = userDetails.PhoneNo,
                    UserId = userDetails.UserId,
                    AccessToken = accessToken,
                    RefreshToken = userDetails.RefreshToken,
                    IsDeviceRegistered = true
            };
            return ApiResponseWithData<LoginResponseModel>.SuccessResponse(response);
        }
        
    }

}
