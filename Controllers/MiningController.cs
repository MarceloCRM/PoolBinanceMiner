using Microsoft.AspNetCore.Mvc;
using PoolBinanceMonitor.Services;

namespace PoolBinanceMonitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiningController : ControllerBase
    {
        private readonly BinanceMiningService _binanceApi;

        public MiningController(BinanceMiningService binanceApi)
        {
            _binanceApi = binanceApi;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus(
            [FromQuery] string algo = "sha256",
            [FromQuery] string userName = "pelminer")
        {
            try
            {
                var json = await _binanceApi.GetUserMiningStatusAsync(algo, userName);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
