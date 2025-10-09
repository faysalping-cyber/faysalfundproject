using FaysalFundsWrapper.Models;

namespace FaysalFundsWrapper.Interfaces
{
    public interface IAuthService
    {

        //Task<string> IsTokenBlacklistedAsync(string request);
        //Task<string> GetDropdowns();
        Task<ApiResponseWithData<LoginResponseModel>> LoginAsync(LoginRequest request);
        Task<ApiResponseWithData<Dictionary<string, List<DropDownDTO>>>> GetDropdowns();
        Task<ApiResponseNoData> SignupAsync(SignupRequest request);
        Task<ApiResponseNoData> ForgotPassword(ForgotPasswordModel request);
        Task<ApiResponseNoData> UsersAppAccountVarification(UserDetailsVerification request);
        Task<ApiResponseNoData> Logout(BlackListTokenRequestModel request);
        Task<string> SetPassword(SetPasswordModel request);
        Task<ApiResponseNoData> ChangePassword(ChangePasswordModel request);
    }
}
