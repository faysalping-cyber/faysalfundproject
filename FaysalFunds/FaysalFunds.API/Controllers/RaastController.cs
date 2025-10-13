using FaysalFunds.Application.DTOs.ExternalAPI;
using FaysalFunds.Infrastructure.ExternalService;
using Microsoft.AspNetCore.Mvc;

namespace FaysalFunds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaastController : ControllerBase
    {
        private readonly FamlInternalService _FamlInternalService;

        public RaastController(FamlInternalService famlInternalService)
        {
            _FamlInternalService = famlInternalService;
        }

        [HttpPost("GenerateRaastId")]
        public async Task<IActionResult> GenerateRaastId(GenerateIbanRequestModel model)
        {
            var response = await _FamlInternalService.GenerateIban(model);
            return Ok(response);
        }

        [HttpPost("GetRaastIdsList")]
        public async Task<IActionResult> GetRaastIdsList(RaastIdsListRequestModel model)
        {
            var response = await _FamlInternalService.GetRaastIdsList(model);
            return Ok(response);
        }
    }
}