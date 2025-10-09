using FaysalFunds.API.Models;
using FaysalFunds.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFunds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private readonly JWTBlackListService _jWTBlackListService;
        private readonly RefreshTokenService _refreshTokenService;

        public JWTController(JWTBlackListService jWTBlackListService, RefreshTokenService refreshTokenService)
        {
            _jWTBlackListService = jWTBlackListService;
            _refreshTokenService = refreshTokenService;
        }
        [HttpPost("IsTokenBlacklistedAsync")]
        public async Task<IActionResult> IsTokenBlacklistedAsync(TokenRequest request)
        {
            var response = await _jWTBlackListService.IsTokenBlacklistedAsync(request.Token);
            return Ok(response);
        }
    }
}
