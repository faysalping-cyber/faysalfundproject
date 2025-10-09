using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Services;
using FaysalFundsWrapper.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FaysalFundsWrapper.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly TokenProviderService _tokenProviderService;
        private readonly IAuthService _authService;
        private readonly IJWTBlackLisService _jWTBlackLisService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly int ExpiryDays = 7;
        public AuthController(IAuthService authService, IJWTBlackLisService jWTBlackLisService, TokenProviderService tokenProviderService, RefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _jWTBlackLisService = jWTBlackLisService;
            _tokenProviderService = tokenProviderService;
            _refreshTokenService = refreshTokenService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Validate request
            request.ValidateAndThrow(new LoginValidation());
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }


        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            // Validate request
            request.ValidateAndThrow(new SignupValidation());

            var response = await _authService.SignupAsync(request);
            return Ok(response);
        }

        [HttpPost("SetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordModel request)
        {
            var response = await _authService.SetPassword(request);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ResetPassword(ForgotPasswordModel request)
        {
            var response = await _authService.ForgotPassword(request);
            return Ok(response);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expiry = jwt.ValidTo;
            var response = await _authService.Logout(new BlackListTokenRequestModel { ExpirationDate = expiry, Token = token });
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("UsersAppAccountVarification")]
        public async Task<IActionResult> UsersAppAccountVarification([FromBody] UserDetailsVerification request)
        {
            var response = await _authService.UsersAppAccountVarification(request);
            return Ok(response);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            model.ValidateAndThrow(new ChangePasswordValidation());

            model.UserId = GetUserIdFromToken();
            var response = await _authService.ChangePassword(model);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequest request)
        {
            var principal = _tokenProviderService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return Ok(ApiResponseNoData.FailureResponse("Invalid token"));
            }
            var email = principal.Identity?.Name ?? principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            if (email == null)
                //throw new ApiException(ApiErrorCodes.Unauthorized, "Invalid token");//
                return Ok(ApiResponseNoData.FailureResponse("Invalid token"));

            var storedToken = await _refreshTokenService.GetByTokenAsync(new TokenRequest { RefreshToken = request.RefreshToken });
            //deseriallizati0om
            var result = JsonConvert.DeserializeObject<SuccessResponse<RefreshTokenResponseModel>>(storedToken);

            // Deserialize the Data object into the specific model
            if (result == null || result.Data.Email != email || result.Data.IsRevoked == 1 || result.Data.ExpiryDate <= DateTime.UtcNow)
                //throw new ApiException(ApiErrorCodes.Unauthorized, "Invalid or expired refresh token");//
                return Ok(ApiResponseNoData.FailureResponse("Invalid or expired refresh token"));
            // Revoke old
            await _refreshTokenService.RevokeUserTokensAsync(new RevokeTokenRequestModel { Email = result.Data.Email });

            // Create new tokens
            var jwtPayload = new JWTPayload { Email = email, };
            var newAccessToken = _tokenProviderService.Create(jwtPayload);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

            await _refreshTokenService.SaveRefreshToken(new RefreshTokenRequestModel
            {
                Email = email,

            });
            SuccessResponse<TokenResponse> successResponse = new()
            {
                StatusCode = 200,
                Data = new TokenResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken },
            };
            return Ok(successResponse);
        }
        [HttpGet("GetDropdowns")]
        public async Task<IActionResult> GetDropdowns()
        {
            var dropdowns = await _authService.GetDropdowns();
            return Ok(dropdowns);
        }

        private long GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return long.Parse(userIdClaim.Value);
        }
    }
}