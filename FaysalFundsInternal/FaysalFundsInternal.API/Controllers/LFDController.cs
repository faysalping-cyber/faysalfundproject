using FaysalFundsInternal.API.ModelValidations;
using FaysalFundsInternal.Infrastructure;
using FaysalFundsInternal.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFundsInternal.API.Controllers
{
    [Route("api/LFD")]
    [ApiController]
    public class LFDController : ControllerBase
    {
        private readonly ILFDService _lfdService;

        public LFDController(ILFDService lfdService)
        {
            _lfdService = lfdService;
        }
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] LFDRequestModel request)
        {
            request.ValidateAndThrow(new LFDRequestModelValidation());
            var response = await _lfdService.Search(request.CNIC);
            return Ok(response);
        }
    }
}
