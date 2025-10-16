using FaysalFundsWrapper.Interfaces;
using FaysalFundsWrapper.Models;
using FaysalFundsWrapper.Models.Raast;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FaysalFundsWrapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaastController : ControllerBase
    {
        private readonly IRaastService _raastService;

        public RaastController(IRaastService raastService)
        {
            _raastService = raastService;
        }

        [HttpPost("GenerateRaastId")]
        public async Task<IActionResult> GenerateRaastId(GenerateRaastIdRequestModel model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            model.CNIC = User.FindFirst(CustomClaimTypes.CNIC).ToString();
            model.CellNo = User.FindFirst(ClaimTypes.MobilePhone).ToString();
            model.AccountOpeningId = long.Parse(userIdClaim.Value);
            var response =await _raastService.GenerateRaastId(model);
            return Ok(response);
        }

        [HttpPost("GetRaastIdsList")]
        public async Task<IActionResult> GetRaastIdsList(RaastIdsListRequestModel model)
        {
            var response = await _raastService.GetRaastIdsList(model);
            return Ok(response);
        }
    }
}
