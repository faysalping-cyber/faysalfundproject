using FaysalFunds.API.Models;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFunds.API.Controllers
{
    [Route("api/RefreshToken")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly RefreshTokenService _refreshTokenService;

        public RefreshTokenController(RefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }


        [HttpPost("SaveRefreshToken")]
        public async Task<IActionResult> SaveRefreshToken(RefreshTokenRequestModel request)
        {
            var response =await _refreshTokenService.SaveRefreshToken(request.Email);
            return Ok(response);
        }

        [HttpPost("RevokeUserTokensAsync")]
        public async Task<IActionResult> RevokeUserTokensAsync(RevokeTokenRequestModel request)
        {
            var response = await _refreshTokenService.RevokeUserTokensAsync(request.Email);
            return Ok(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRefreshRequest request)
        {
            var response = await _refreshTokenService.GetByTokenAsync(request.RefreshToken);
            return Ok(response);
        }
    }
}
