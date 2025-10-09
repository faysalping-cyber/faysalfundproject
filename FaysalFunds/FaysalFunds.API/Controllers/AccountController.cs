using FaysalFunds.API.Models;
using FaysalFunds.Application.DTOs;
using FaysalFunds.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFunds.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly JWTBlackListService _jWTBlackListService;
        private readonly DropdownService _dropdownService;
        public AccountController(AccountService accountService, JWTBlackListService jWTBlackListService, DropdownService dropdownService)
        {
            _accountService = accountService;
            _jWTBlackListService = jWTBlackListService;
            _dropdownService = dropdownService;
        }
        [HttpPost("Signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(Signup signup)
        {
            var response = await _accountService.Signup(signup);
            return Ok(response);
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login model)
        {
           var response = await _accountService.Login(model);
            return Ok(response);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var response = await _accountService.ResetPassword(model);
            return Ok(response);
        }

        [HttpPost("ChnagePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var response = await _accountService.ChangePassword(model);
            return Ok(response);
        }
        [HttpPost("SetPassword")]
        public async Task<IActionResult> SetPassword(SetPasswordModel model)
        {
            var userId = await _accountService.GetUserIdByEmail(model.Email);
            var response = await _accountService.SetPassword(userId.Data, model.Password);
            return Ok(response);
        }
        [HttpPost("UsersAppAccountVarification")]
        public async Task<IActionResult> UsersAppAccountVarification(UserDetailsVerificationModel model)
        {
            var response = await _accountService.UsersAppAccountVarification(model);
            return Ok(response);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> BlacklistTokenAsync(BlaclListToken model)
        {
            var response = await _jWTBlackListService.BlacklistTokenAsync(model.Token, model.ExpirationDate);
            return Ok(response);
        }
        [HttpGet("GetDropdowns")]
        public async Task<IActionResult>GetDropdownsList()
        {
            var dropdowns = await _dropdownService.GetDropdownsAsync();
            return Ok(dropdowns);
        }
    }
}
