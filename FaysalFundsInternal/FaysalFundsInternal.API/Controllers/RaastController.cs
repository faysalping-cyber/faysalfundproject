using FaysalFundsInternal.API.ModelValidations;
using FaysalFundsInternal.Infrastructure.DTOs;
using FaysalFundsInternal.Infrastructure.DTOs.Raast;
using FaysalFundsInternal.Infrastructure.Raast;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFundsInternal.API.Controllers
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


        [HttpPost("GenerateIban")]
        public async Task<IActionResult> Search([FromBody] GenerateIbanRequestModel request)
        {
            var response = await _raastService.GenerateIban(request);
            return Ok(response);
        }


        [HttpPost("GetIbanList")]
        public async Task<IActionResult> GetIbanList([FromBody] ListIbanRequestModel request)
        {
            var response = await _raastService.GetIbanList(request);
            return Ok(response);
        }
    }
}
